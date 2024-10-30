using Base.Core.Entities;
using Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Student : EntityObject, IUser
    {
        public string Username {get; set;}
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<Assignments> Assignments { get; set; }
    }
}
