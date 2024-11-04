using Base.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Tag : EntityObject
    {
        public string Name { get; set; }
        public Tag(string name)
        {
            Name = name;
        }
    }
}
