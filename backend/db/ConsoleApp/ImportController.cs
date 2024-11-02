using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class ImportController
    {
        public static ImportData ImportDemoData()
        {

            User user1 = new User
            {
                Username = "if200183",
                Firstname = "Florian",
                Lastname = "Hagmair",
                IsTeacher = true,
            };

            User user2 = new User
            {
                Username = "if200182",
                Firstname = "David",
                Lastname = "Pröller",
                IsTeacher = false,
            };

            User user3 = new User
            {
                Username = "if200xx1",
                Firstname = "Travis",
                Lastname = "Scott",
                IsTeacher = true,
            };

            User user4 = new User
            {
                Username = "if200xx2",
                Firstname = "Hans",
                Lastname = "Neumüller",
                IsTeacher = false,
            };

            User user5 = new User
            {
                Username = "if200xx3",
                Firstname = "Max",
                Lastname = "Mustermann",
                IsTeacher = false,
            };

            User user6 = new User
            {
                Username = "if200xx4",
                Firstname = "Gerd",
                Lastname = "Müller",
                IsTeacher = false,
            };

            User user7 = new User
            {
                Username = "if200xx5",
                Firstname = "Tomas",
                Lastname = "Brolin",
                IsTeacher = false,
            };

            Tag tag1 = new("Class1");

            Tag tag2 = new("POSE");

            Tag tag3 = new("WMC");

            Tag tag4 = new("Class2");

            Exercise exercise1 = new Exercise
            {
                Name = "Addition",
                Description = "Implement the a addition calculator",
                Language = Language.CSharp,
                Tags = new List<Tag>() { tag1, tag2 },
                
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            exercise1.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise1,
                        ExerciseId = exercise1.Id
                    };

            exercise1.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise1.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise1.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise1.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise1.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise1.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise1.ArrayOfSnippets.Id
                    }
                };

            exercise1.TeacherId = user1.Id;
            exercise1.Teacher = user1;

            Exercise exercise2 = new Exercise
            {
                Name = "PasswordChecker",
                Description = "Implement a password checker",
                Language = Language.TypeScript,
                Tags = new List<Tag>() { tag3, tag4 },
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };
            exercise2.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise2,
                        ExerciseId = exercise2.Id
                    };

            exercise2.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "export function CheckPassword(password: string): boolean{",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise2.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise2.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new Error('Method not implemented.');",
                        ReadonlySection = false,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise2.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise2.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise2.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise2.ArrayOfSnippets.Id
                    }
                };

            exercise2.TeacherId = user1.Id;
            exercise2.Teacher = user1;

            Exercise exercise3 = new Exercise
            {
                Name = "SubtractionEmpty",
                Description = "Implement the a subtraction calculator",
                Language = Language.CSharp,
                Tags = new List<Tag>() { tag1, tag3 },

                
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            exercise3.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise3,
                        ExerciseId = exercise3.Id
                    };

            exercise3.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Program { static void Main(string[] args) {} public static int Subtract(int a, int b){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise3.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise3.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise3.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise3.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise3.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise3.ArrayOfSnippets.Id
                    }
                };

            exercise3.TeacherId = user1.Id;
            exercise3.Teacher = user1;

            Assignments assignment1 = new Assignments
            {
                Name = "Assignment1",
                Exercise = exercise1,
                ExerciseId = exercise1.Id,
                DateDue = DateTime.Now,
                TeacherId = user1.Id,
                Teacher = user1
            };

            Assignments assignment2 = new Assignments
            {
                Name = "Assignment2",
                Exercise = exercise2,
                ExerciseId = exercise2.Id,
                DateDue = DateTime.Now,
                TeacherId = user1.Id,
                Teacher = user1
            };

            var assignmentUsers = new List<AssignmentUser>
            {
                new AssignmentUser { Assignment = assignment1, User = user2 },
                new AssignmentUser { Assignment = assignment1, User = user4 },
                new AssignmentUser { Assignment = assignment1, User = user5 },
                new AssignmentUser { Assignment = assignment1, User = user6 },
                new AssignmentUser { Assignment = assignment1, User = user7 },

                new AssignmentUser { Assignment = assignment2, User = user2 },
                new AssignmentUser { Assignment = assignment2, User = user4 },
                new AssignmentUser { Assignment = assignment2, User = user5 }
            };

            var users = new List<User> { user1, user2, user3, user4, user5, user6, user7};

            var tags = new List<Tag> { tag1, tag2, tag3, tag4 };

            var exercises = new List<Exercise> { exercise1, exercise2, exercise3 };

            

            return new ImportData
            {
                AssignmentUsers = assignmentUsers,
                Tags = tags,
                Users = users,
                Exercises = exercises,
                Assignments = new List<Assignments> { assignment1, assignment2 },
                ArrayOfSnippets = new List<ArrayOfSnippets> { exercise1.ArrayOfSnippets, exercise2.ArrayOfSnippets, exercise3.ArrayOfSnippets },
                Snippets = new List<Snippet> { exercise1.ArrayOfSnippets.Snippets[0], exercise1.ArrayOfSnippets.Snippets[1], exercise1.ArrayOfSnippets.Snippets[2], exercise2.ArrayOfSnippets.Snippets[0], exercise2.ArrayOfSnippets.Snippets[1], exercise2.ArrayOfSnippets.Snippets[2], exercise3.ArrayOfSnippets.Snippets[0], exercise3.ArrayOfSnippets.Snippets[1], exercise3.ArrayOfSnippets.Snippets[2] }
            };
        }
    }
}
