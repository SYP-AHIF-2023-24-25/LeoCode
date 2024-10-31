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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateUser(string username, string firstName, string lastName, bool isTeacher)
        {
            User user = new User
            {
                Username = username,
                Firstname = firstName,
                Lastname = lastName,
                IsTeacher = isTeacher
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public User GetByUsername(string username)
        {
            return _dbContext.Users.SingleOrDefault(u => u.Username == username)!;
        }
    }
}
