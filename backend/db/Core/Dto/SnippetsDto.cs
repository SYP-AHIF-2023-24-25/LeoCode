using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public record class SnippetDto(string Code, bool ReadonlySection, string FileName);

    public class AssignmentDto
    {
        public string AssignmentName { get; set; }
        public string Language { get; set; }
        public DateTime DueDate { get; set; }
        public string ExerciseName { get; set; }
        public ExerciseAssignemntDto Exercise { get; set; }
        public TeacherDto Teacher { get; set; }
        public List<StudentDto> Students { get; set; }
    }

    public class ExerciseAssignemntDto
    {
        public string ExerciseName { get; set; }
        public string Language { get; set; }
        public string[] Tags { get; set; }
    }

    

    public class TeacherDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
    }

    public class StudentDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
    }
}
