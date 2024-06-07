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

        public async Task<List<ExerciseDto>> GetExersiceByUsernameAsync(User user, string? exerciseName)
        {
            IQueryable<Exercise> exerciseQuery = _dbContext.Exercises;

            if (exerciseName != null)
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.UserId == user.Id && exercise.Name == exerciseName);
            }
            else
            {
                exerciseQuery = exerciseQuery.Where(exercise => exercise.UserId == user.Id);
            }

            return await exerciseQuery.Select(exercise => new ExerciseDto(
            exercise.Name,
            exercise.Description,
            ((Language)exercise.Language).ToString(),
            exercise.Year,
            ((Subject)exercise.Subject).ToString(),
            new ArrayOfSnippetsDto(
                exercise.ArrayOfSnippets.Snippets.Select(snippet => new SnippetDto(
                    snippet.Code,
                    snippet.ReadonlySection,
                    snippet.FileName)).ToArray()
            )
            )).ToListAsync();

        }
    }
}
