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
        public Exercise? Exercise { get; set; }
        public int ExerciseId { get; set; }
    }
}
