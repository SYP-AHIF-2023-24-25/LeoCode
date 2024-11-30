using Base.Core.Contracts;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        public Teacher GetByUsername(string username);
        public void CreateUser(string username, string firstName, string lastName);
        public Task<List<Teacher>> GetAllUsers();
    }
}
