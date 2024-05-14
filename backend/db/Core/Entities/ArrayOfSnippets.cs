using Base.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ArrayOfSnippets : EntityObject
    {
        public List<Snippet> Snippets { get; set; } = [];

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public int UserId { get; set; }
    }
}
