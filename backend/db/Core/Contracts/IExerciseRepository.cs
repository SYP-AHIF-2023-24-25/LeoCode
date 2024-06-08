using Base.Core.Contracts;
using Core.Dto;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IExerciseRepository : IGenericRepository<Exercise>
    {
        public Task<List<Exercise>> GetExersiceByUsernameAsync(User user, string? exerciseName);
    }
}
