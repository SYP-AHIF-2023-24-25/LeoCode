using Base.Core.Entities;
using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : EntityObject
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsTeacher { get; set; }

        // Replace Assignments with AssignmentUsers for the many-to-many relationship
        public List<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();
    }
}
