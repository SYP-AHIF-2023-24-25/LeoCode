using Base.Persistence;
using Core.Contracts;
using Core.Dto;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ExerciseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Exercise>> GetAll()
        {
            return await _dbContext.Exercises
                .Include(e => e.Teacher)
                .Include(e => e.Tags)
                .Include(e => e.ArrayOfSnippets)
                .ThenInclude(e => e.Snippets)
                .ToListAsync();
        }

        public async Task<ExerciseDto> GetExerciseForStudentAssignment(string language, string exerciseName, string student)
        {
            Exercise exercise = _dbContext.Exercises
                .Include(e => e.Teacher)
                .Include(e => e.Tags)
                .Include(e => e.ArrayOfSnippets)
                .ThenInclude(e => e.Snippets)
                .Where(e => e.Name == exerciseName
                    && e.Language == (Language)Enum.Parse(typeof(Language), language)
                    && e.Student.Username == student)
                .SingleOrDefault();

            if (exercise != null)
            {
                // Exercise erfolgreich gefunden, dann DTO erstellen
                var exerciseDto = new ExerciseDto(
                    exercise.Name,
                    exercise.Teacher.Username,
                    exercise.Description,
                    ((Language)exercise.Language).ToString(),
                    exercise.Tags.Select(tag => tag.Name).ToArray(),
                    exercise.ArrayOfSnippets.Snippets.Select(snippet => new SnippetDto(
                        snippet.Code,
                        snippet.ReadonlySection,
                        snippet.FileName)).ToArray(),
                    exercise.DateCreated,
                    exercise.DateUpdated
                );

                return exerciseDto;  // Beispiel für Rückgabe des DTOs
            }
            else
            {
                // Falls kein Exercise gefunden wurde
                return null;
            }
        }

        public async Task<List<Exercise>> GetExersiceByUsernameStudentAsync(Student student, string? exerciseName)
        {
            IQueryable<Exercise> exerciseQuery = _dbContext.Exercises
                .Include(exercise => exercise.Student)
                .Include(exercise => exercise.Tags)
                .Include(exercise => exercise.ArrayOfSnippets)
                .ThenInclude(arrayOfSnippets => arrayOfSnippets.Snippets);

            if (exerciseName != null)
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.Student.Username == student.Username && exercise.Name == exerciseName);
            }
            else
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.StudentId == student.Id);
            }

            return await exerciseQuery.ToListAsync();
        }

        public async Task<List<Exercise>> GetExersiceByUsernameTeacherAsync(Teacher teacher, string? exerciseName)
        {
            IQueryable<Exercise> exerciseQuery = _dbContext.Exercises
                .Include(exercise => exercise.Student)
                .Include(exercise => exercise.Tags)
                .Include(exercise => exercise.ArrayOfSnippets)
                .ThenInclude(arrayOfSnippets => arrayOfSnippets.Snippets);

            if (exerciseName != null)
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.Teacher.Username == teacher.Username && exercise.Name == exerciseName);
            }
            else
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.TeacherId == teacher.Id);
            }

            return await exerciseQuery.ToListAsync();
        }
    }
}
