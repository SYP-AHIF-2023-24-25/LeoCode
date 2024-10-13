using Base.Core.Contracts;
using Base.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Assignments : EntityObject
    {
        public Exercise Exercise { get; set; }
        public string Creator { get; set; }
        public DateTime DateDue { get; set; }
        public User[] Students { get; set; }
        public string Name { get; set; }
    }
}
