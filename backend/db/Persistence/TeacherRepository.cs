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
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TeacherRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateUser(string username, string firstName, string lastName)
        {
            Teacher user = new Teacher
            {
                Username = username,
                Firstname = firstName,
                Lastname = lastName,
            };
            _dbContext.Teacher.Add(user);
            _dbContext.SaveChanges();
        }

        public async Task<List<Teacher>> GetAllUsers()
        {
            return await _dbContext.Teacher.ToListAsync();
        }

        public Teacher GetByUsername(string username)
        {
            return _dbContext.Teacher.SingleOrDefault(u => u.Username == username)!;
        }
    }
}
