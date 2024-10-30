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
                .Include(exercise => exercise.ArrayOfSnippets)
                .ThenInclude(arrayOfSnippets => arrayOfSnippets.Snippets)
                .ToListAsync();
        }

        public async Task<List<Exercise>> GetExersiceByUsernameAsync(Teacher teacher, string? exerciseName)
        {
            IQueryable<Exercise> exerciseQuery = _dbContext.Exercises
                .Include(exercise => exercise.ArrayOfSnippets)
                .ThenInclude(arrayOfSnippets => arrayOfSnippets.Snippets);

            if (exerciseName != null)
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.TeacherId == teacher.Id && exercise.Name == exerciseName);
            }
            else
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.TeacherId == teacher.Id);
            }

            return await exerciseQuery.ToListAsync();
        }
    }
}
