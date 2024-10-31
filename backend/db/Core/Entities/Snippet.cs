using Base.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Snippet : EntityObject
    {
        
        public string Code { get; set; }
        public bool ReadonlySection { get; set; }
        public string FileName { get; set; }

        [ForeignKey(nameof(ArrayOfSnippetsId))]
        public ArrayOfSnippets? ArrayOfSnippets { get; set; }
        public int ArrayOfSnippetsId { get; set; }
        
        
    }
}
