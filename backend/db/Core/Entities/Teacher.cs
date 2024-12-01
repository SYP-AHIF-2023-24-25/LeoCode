using Base.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Teacher : EntityObject
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        // Replace Assignments with AssignmentUsers for the many-to-many relationship
        public List<AssignmentUser> AssignmentUsers { get; set; } = new List<AssignmentUser>();
    }
}
