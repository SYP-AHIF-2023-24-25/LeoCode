using Base.Core.Entities;
using Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Exercise : EntityObject
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
