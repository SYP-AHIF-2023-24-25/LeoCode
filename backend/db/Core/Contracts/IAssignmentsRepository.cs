using Base.Core.Contracts;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Contracts
{
    public interface IAssignmentsRepository : IGenericRepository<Assignments>
    {
        public Task<List<Assignments>> GetAll();
        public Task<Assignments> GetOneAssignment(string Creator,string Name);
        public void CreateAssignment(string exerciseName, string creator, DateTime dateDue, string Name);

        public Task<IEnumerable<Assignments>> GetAssignmentsByUsername(string username);
    }
}
