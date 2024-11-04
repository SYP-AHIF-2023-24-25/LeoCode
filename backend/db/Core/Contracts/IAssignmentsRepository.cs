using Base.Core.Contracts;
using Core.Dto;
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
        public Task<List<AssignmentDto>> GetAll(string? username);
        public Task<Assignments> GetOneAssignment(string Creator, string Name);
        public string CreateAssignment(string exerciseName, string creator, DateTime dateDue, string Name);
        void JoinAssignment(int assignmentId, string ifStudentName);
        Task<List<User>?> GetAssignmentUsers(int assignmentId);
        public Task<IEnumerable<Assignments>> GetAssignmentsByUsername(string username);

    }
}
