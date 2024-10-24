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
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }
        public string[] Tags { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public int UserId { get; set; }

        public ArrayOfSnippets ArrayOfSnippets { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
