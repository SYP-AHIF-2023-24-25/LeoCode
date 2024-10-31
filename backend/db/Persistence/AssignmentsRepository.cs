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
            User teacher = _dbContext.Users.FirstOrDefault(teacher => teacher.Username == creator);
            Exercise exercise = _dbContext.Exercises.FirstOrDefault(exercise => exercise.Name == exerciseName && exercise.Teacher.Username == teacher.Username);
            Assignments assignment = new Assignments
            {
                Exercise = exercise,
                ExerciseId = exercise!.Id,
                Students = [],
                DateDue = dateDue,
                Name = Name
            };
            _dbContext.Assignments.Add(assignment);
            _dbContext.SaveChanges();
        }

        public async Task<List<Assignments>> GetAll()
        {
            return await _dbContext.Assignments.Include(a => a.Teacher).Include(a => a.Students).Include(a => a.Exercise).ThenInclude(a => a.Tags).ToListAsync();
        }

        public async Task<Assignments> GetOneAssignment(string Creator, string Name)
        {
            User teacher = _dbContext.Users.FirstOrDefault(teacher => teacher.Username == Creator);
            return await _dbContext.Assignments.Include(a => a.Students).Include(a => a.Teacher).Include(a => a.Exercise).ThenInclude(e => e.Tags).FirstOrDefaultAsync(assignment => assignment.Teacher.Username == teacher.Username && assignment.Name == Name);

        }
    }
}
