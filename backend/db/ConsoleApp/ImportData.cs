using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class ImportData
    {

        public List<Exercise> Exercises { get; set; } = [];
        public List<Student> Students { get; set; } = [];
        public List<Teacher> Teachers { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];


        public List<ArrayOfSnippets> ArrayOfSnippets { get; set; } = [];
        public List<Snippet> Snippets { get; set; } = [];
    }
}
