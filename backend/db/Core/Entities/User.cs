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
        public string Password { get; set; }
        public ICollection<Exercise> Exercises { get; set; } = [];
        public ICollection<ArrayOfSnippets> ArrayOfSnippets { get; set; } = [];
    }
}
