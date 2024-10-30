using Base.Persistence;
using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
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

        public void CreateAssignment(string exerciseName, string creator, DateTime dateDue, string Name)
        {
            Teacher teacher = _dbContext.Teachers.FirstOrDefault(teacher => teacher.Username == creator);
            Exercise exercise = _dbContext.Exercises.FirstOrDefault(exercise => exercise.Name == exerciseName && exercise.Teacher.Username == teacher.Username);
            Assignments assignment = new Assignments
            {
                Exercise = exercise,
                ExerciseId = exercise.Id,
                Teacher = teacher,
                TeacherId = teacher.Id,
                DateDue = dateDue,
                Name = Name
            };
            _dbContext.Assignments.Add(assignment);
            _dbContext.SaveChanges();
        }

        public async Task<List<Assignments>> GetAll()
        {
            return await _dbContext.Assignments.ToListAsync();
        }

        public async Task<Assignments> GetOneAssignment(string Creator, string Name)
        {
            Teacher teacher = _dbContext.Teachers.FirstOrDefault(teacher => teacher.Username == Creator);
            return await _dbContext.Assignments.FirstOrDefaultAsync(assignment => assignment.Teacher.Username == teacher.Username && assignment.Name == Name);
        }
    }
}
