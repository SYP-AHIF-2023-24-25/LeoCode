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
        public List<Snippet> Snippets { get; set; } = new List<Snippet>();

        [ForeignKey(nameof(ExerciseId))]
        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
        
    }
}
