using Base.Persistence;
using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class AssignmentsRepository : GenericRepository<Assignments>, IAssignmentsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AssignmentsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public string CreateAssignment(string exerciseName, string creator, DateTime dateDue, string Name)
        {
            Teacher? teacher = _dbContext.Teacher.FirstOrDefault(teacher => teacher.Username == creator);
            if (teacher == null)
            {
                throw new ArgumentException("Creator not found", nameof(creator));
            }
            Exercise? exercise = _dbContext.Exercises
                .Include(e => e.Teacher)
                .FirstOrDefault(exercise => exercise.Name == exerciseName);
            if (exercise == null)
            {
                throw new ArgumentException("Exercise not found", nameof(exerciseName));
            }

            Assignments assignment = new Assignments
            {
                Exercise = exercise,
                ExerciseId = exercise.Id,
                DateDue = dateDue,
                Name = Name,
                Teacher = teacher,
                TeacherId = teacher.Id

            };
            _dbContext.Assignments.Add(assignment);
            _dbContext.SaveChanges();

            // Generate the link for students to join
            string link = GenerateAssignmentLink(assignment.Id);

            return link; // Return the generated link
        }

        private string GenerateAssignmentLink(int assignmentId)
        {
            string baseUrl = "http://localhost:4200/join-assignment"; // Base URL of the Angular frontend
            return $"{baseUrl}/{assignmentId}"; // Construct the full link with the assignmentId as a path parameter
        }




        public async Task<List<AssignmentDto>> GetAll(string? username)
        {
            IQueryable<Assignments> query = _dbContext.Assignments
        .Include(a => a.Teacher)
        .Include(a => a.Exercise)
            .ThenInclude(e => e.Tags)
        .Include(a => a.AssignmentUsers)
            .ThenInclude(au => au.Student);

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(a => a.Teacher.Username == username).OrderBy(a => a.DateDue);
            }

            var assignments = await query.ToListAsync();

            // Alle Exercises, die einem Schüler zugeordnet sind
            var allExercises = _dbContext.Exercises
                .Include(e => e.Student) // Include für den direkten Zugriff auf den Schüler
                .Where(e => e.Student != null) // Nur Exercises mit zugeordneten Schülern
                .ToList();

            // Transformiere die Ergebnisse in DTOs
            var result = assignments.Select(a => new AssignmentDto
            {
                AssignmentName = a.Name,
                DueDate = a.DateDue,
                Exercise = new ExerciseAssignmentDto
                {
                    Language = a.Exercise.Language.ToString(),
                    ExerciseName = a.Exercise.Name,
                    Tags = a.Exercise.Tags.Select(t => t.Name).ToArray()
                },
                Teacher = new TeacherDto
                {
                    Firstname = a.Teacher.Firstname,
                    Lastname = a.Teacher.Lastname,
                    Username = a.Teacher.Username
                },
                Students = a.AssignmentUsers.Select(au =>
                {
                    // Suche die Testergebnisse des Studenten in `allExercises`
                    var studentExercise = allExercises.FirstOrDefault(e => e.Student.Username == au.Student.Username && e.Name == au.Assignment.Exercise.Name);

                    return new StudentDto
                    {
                        Firstname = au.Student.Firstname,
                        Lastname = au.Student.Lastname,
                        Username = au.Student.Username,
                        TotalTests = studentExercise?.TotalTests ?? 0,   // Standardwert 0 bei fehlendem Exercise
                        PassedTests = studentExercise?.PassedTests ?? 0,
                        FailedTests = studentExercise?.FailedTests ?? 0
                    };
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<Assignments?> GetOneAssignment(string Creator, string Name)
        {
            Teacher? teacher = _dbContext.Teacher.FirstOrDefault(teacher => teacher.Username == Creator);
            if (teacher == null)
            {
                return null;
            }

            return await _dbContext.Assignments
                //.Include(a => a.Students)
                .Include(a => a.Teacher)
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Tags)
                .FirstOrDefaultAsync(assignment => assignment.Teacher.Username == teacher.Username && assignment.Name == Name);
        }

        public void JoinAssignment(int assignmentId, string ifStudentName)
        {
            var user = _dbContext.Student.FirstOrDefault(u => u.Username == ifStudentName);
            if (user == null)
            {
                throw new ArgumentException("Student not found", nameof(ifStudentName));
            }

            AssignmentUser assignmentUser = new AssignmentUser
            {
                Assignment = _dbContext.Assignments.FirstOrDefault(a => a.Id == assignmentId),
                Student = user,
                AssignmentId = assignmentId,
                StudentId = user.Id
            };
            _dbContext.Assignments.FirstOrDefault(a => a.Id == assignmentId)?.AssignmentUsers.Add(assignmentUser);
            _dbContext.SaveChanges();
        }

        public async Task<List<Student>?> GetAssignmentUsers(int assignmentId)
        {
            var users = await _dbContext.Assignments
                .Where(a => a.Id == assignmentId)
                .SelectMany(a => a.AssignmentUsers)
                .Select(au => au.Student)
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<Assignments>> GetAssignmentsByUsername(string username)
        {
            return await _dbContext.Assignments
           .Include(a => a.AssignmentUsers)
           .Include(a => a.Teacher)
           .Include(a => a.Exercise)
           .ThenInclude(e => e.Tags)
           .Include(a => a.Exercise)
                .ThenInclude(e => e.ArrayOfSnippets)
                .ThenInclude(a => a.Snippets)
           .Include(a => a.Exercise)
               .ThenInclude(e => e.Teacher)
           .Where(a => a.AssignmentUsers.Any(s => s.Student.Username == username))
           .ToListAsync();
        }
    }
}
