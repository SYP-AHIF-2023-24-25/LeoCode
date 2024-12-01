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
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateUser(string username, string firstName, string lastName)
        {
            Student user = new Student
            {
                Username = username,
                Firstname = firstName,
                Lastname = lastName,
            };
            _dbContext.Student.Add(user);
            _dbContext.SaveChanges();
        }

        public async Task<List<Student>> GetAllUsers()
        {
            return await _dbContext.Student.ToListAsync();
        }

        public Student GetByUsername(string username)
        {
            return _dbContext.Student.SingleOrDefault(u => u.Username == username)!;
        }
    }
}
