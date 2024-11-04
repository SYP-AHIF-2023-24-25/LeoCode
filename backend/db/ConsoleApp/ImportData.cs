using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class ImportData
    {

        public List<Exercise> Exercises { get; set; } = [];
        public List<User> Users { get; set; } = [];
        public List<Tag> Tags { get; set; } = [];
        public List<Assignments> Assignments { get; set; } = [];


        public List<ArrayOfSnippets> ArrayOfSnippets { get; set; } = [];
        public List<Snippet> Snippets { get; set; } = [];

        public List<AssignmentUser> AssignmentUsers { get; set; } = [];
    }
}
