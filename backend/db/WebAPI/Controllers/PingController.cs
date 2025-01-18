using Core.Contracts;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ImportData
    {

        public List<Exercise> Exercises { get; set; } = [];
        public List<Teacher> Teachers { get; set; }
        public List<Student> Students { get; set; }
        public List<Tag> Tags { get; set; } = [];
        public List<Assignments> Assignments { get; set; } = [];


        public List<ArrayOfSnippets> ArrayOfSnippets { get; set; } = [];
        public List<Snippet> Snippets { get; set; } = [];

        public List<AssignmentUser> AssignmentUsers { get; set; } = [];
    }
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPatch]
        public async Task<ActionResult<string>> Patch()
        {
            await _unitOfWork.DeleteDatabaseAsync();
            await _unitOfWork.MigrateDatabaseAsync();

            Console.WriteLine("Read data from file ...");

            var data = ImportDemoData();

            await _unitOfWork.Student.AddRangeAsync(data.Students);
            await _unitOfWork.Teacher.AddRangeAsync(data.Teachers);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.Tags.AddRangeAsync(data.Tags);
            await _unitOfWork.Exercises.AddRangeAsync(data.Exercises);
            await _unitOfWork.ArrayOfSnippets.AddRangeAsync(data.ArrayOfSnippets);
            await _unitOfWork.Snippets.AddRangeAsync(data.Snippets);
            await _unitOfWork.Assignments.AddRangeAsync(data.Assignments);
            await _unitOfWork.AssignmentUser.AddRangeAsync(data.AssignmentUsers);

            await _unitOfWork.SaveChangesAsync();

            return Ok("Database created, migrated and demo data inserted");
        }

    public static ImportData ImportDemoData()
        {

            Teacher user1 = new Teacher
            {
                Username = "if200183",
                Firstname = "Florian",
                Lastname = "Hagmair"
            };

            Student user2 = new Student
            {
                Username = "if200182",
                Firstname = "David",
                Lastname = "Pröller"
            };

            Student user3 = new Student
            {
                Username = "if200104",
                Firstname = "Christian",
                Lastname = "Ekhator"
            };

            Student user4 = new Student
            {
                Username = "if200177",
                Firstname = "Samuel",
                Lastname = "Atzlesberger"
            };

            Student user5 = new Student
            {
                Username = "if200145",
                Firstname = "Michael",
                Lastname = "Werner"
            };

            Student user6 = new Student
            {
                Username = "if200107",
                Firstname = "Marcus",
                Lastname = "Rabeder"
            };

            Tag tag1 = new("Class1");

            Tag tag2 = new("POSE");

            Tag tag3 = new("WMC");

            Tag tag4 = new("Class2");

            Tag tag5 = new("SYP");

            Tag tag6 = new("Class3");

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
                Tags = new List<Tag>() { tag5, tag6 },


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

            Exercise exercise4 = new Exercise
            {
                Name = "Addition",
                Description = "Implement the a addition calculator",
                Language = Language.CSharp,
                Tags = [],

                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 3,
                FailedTests = 0,
                TotalTests = 3,
            };

            exercise4.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise4,
                        ExerciseId = exercise4.Id
                    };

            exercise4.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise4.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise4.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise4.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise4.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise4.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise4.ArrayOfSnippets.Id
                    }
                };

            exercise4.TeacherId = user1.Id;
            exercise4.Teacher = user1;
            exercise4.StudentId = user2.Id;
            exercise4.Student = user2;

            Exercise exercise5 = new Exercise
            {
                Name = "Addition",
                Description = "Implement the a addition calculator",
                Language = Language.CSharp,
                Tags = [],

                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 2,
                FailedTests = 1,
                TotalTests = 3,
            };

            exercise5.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise5,
                        ExerciseId = exercise5.Id
                    };

            exercise5.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise5.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise5.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise5.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise5.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise5.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise5.ArrayOfSnippets.Id
                    }
                };

            exercise5.TeacherId = user1.Id;
            exercise5.Teacher = user1;
            exercise5.StudentId = user3.Id;
            exercise5.Student = user3;

            Exercise exercise6 = new Exercise
            {
                Name = "Addition",
                Description = "Implement the a addition calculator",
                Language = Language.CSharp,
                Tags = [],

                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 3,
                FailedTests = 0,
                TotalTests = 3,
            };

            exercise6.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise6,
                        ExerciseId = exercise6.Id
                    };

            exercise6.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise6.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise6.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise6.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise6.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise6.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise6.ArrayOfSnippets.Id
                    }
                };

            exercise6.TeacherId = user1.Id;
            exercise6.Teacher = user1;
            exercise6.StudentId = user4.Id;
            exercise6.Student = user4;

            Exercise exercise7 = new Exercise
            {
                Name = "Addition",
                Description = "Implement the a addition calculator",
                Language = Language.CSharp,
                Tags = [],

                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 3,
                FailedTests = 0,
                TotalTests = 3,
            };

            exercise7.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise7,
                        ExerciseId = exercise7.Id
                    };

            exercise7.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise7.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise7.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise7.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise7.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise7.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise7.ArrayOfSnippets.Id
                    }
                };

            exercise7.TeacherId = user1.Id;
            exercise7.Teacher = user1;
            exercise7.StudentId = user5.Id;
            exercise7.Student = user5;

            Exercise exercise8 = new Exercise
            {
                Name = "Addition",
                Description = "Implement the a addition calculator",
                Language = Language.CSharp,
                Tags = [],

                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 0,
                FailedTests = 3,
                TotalTests = 3,
            };

            exercise8.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise8,
                        ExerciseId = exercise8.Id
                    };

            exercise8.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise8.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise8.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new NotImplementedException();",
                        ReadonlySection = false,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise8.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise8.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}}",
                        ReadonlySection = true,
                        FileName = "Program.cs",
                        ArrayOfSnippets = exercise8.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise8.ArrayOfSnippets.Id
                    }
                };

            exercise8.TeacherId = user1.Id;
            exercise8.Teacher = user1;
            exercise8.StudentId = user6.Id;
            exercise8.Student = user6;

            Exercise exercise9 = new Exercise
            {
                Name = "PasswordChecker",
                Description = "Implement a password checker",
                Language = Language.TypeScript,
                Tags = [],
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 3,
                FailedTests = 0,
                TotalTests = 3,
            };

            exercise9.ArrayOfSnippets =
                    new ArrayOfSnippets
                    {
                        Exercise = exercise9,
                        ExerciseId = exercise9.Id
                    };

            exercise9.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "export function CheckPassword(password: string): boolean{",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise9.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise9.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new Error('Method not implemented.');",
                        ReadonlySection = false,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise9.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise9.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise9.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise9.ArrayOfSnippets.Id
                    }
                };

            exercise9.TeacherId = user1.Id;
            exercise9.Teacher = user1;
            exercise9.StudentId = user2.Id;
            exercise9.Student = user2;

            Exercise exercise10 = new Exercise
            {
                Name = "PasswordChecker",
                Description = "Implement a password checker",
                Language = Language.TypeScript,
                Tags = [],
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 2,
                FailedTests = 1,
                TotalTests = 3,
            };

            exercise10.ArrayOfSnippets =
                   new ArrayOfSnippets
                   {
                       Exercise = exercise10,
                       ExerciseId = exercise10.Id
                   };

            exercise10.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "export function CheckPassword(password: string): boolean{",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise10.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise10.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new Error('Method not implemented.');",
                        ReadonlySection = false,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise10.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise10.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise10.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise10.ArrayOfSnippets.Id
                    }
                };

            exercise10.TeacherId = user1.Id;
            exercise10.Teacher = user1;
            exercise10.StudentId = user3.Id;
            exercise10.Student = user3;

            Exercise exercise11 = new Exercise
            {
                Name = "PasswordChecker",
                Description = "Implement a password checker",
                Language = Language.TypeScript,
                Tags = [],
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                PassedTests = 3,
                FailedTests = 0,
                TotalTests = 3,
            };

            exercise11.ArrayOfSnippets =
                   new ArrayOfSnippets
                   {
                       Exercise = exercise11,
                       ExerciseId = exercise11.Id
                   };

            exercise11.ArrayOfSnippets.Snippets = new List<Snippet>
                {
                    new Snippet
                    {
                        Code = "export function CheckPassword(password: string): boolean{",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise11.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise11.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "throw new Error('Method not implemented.');",
                        ReadonlySection = false,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise11.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise11.ArrayOfSnippets.Id
                    },
                    new Snippet
                    {
                        Code = "}",
                        ReadonlySection = true,
                        FileName = "passwordChecker.ts",
                        ArrayOfSnippets = exercise11.ArrayOfSnippets,
                        ArrayOfSnippetsId = exercise11.ArrayOfSnippets.Id
                    }
                };

            exercise11.TeacherId = user1.Id;
            exercise11.Teacher = user1;
            exercise11.StudentId = user4.Id;
            exercise11.Student = user4;


            Assignments assignment1 = new Assignments
            {
                Name = "Assignment Addition",
                Exercise = exercise1,
                ExerciseId = exercise1.Id,
                DateDue = new DateTime(2024, 11, 18),
                TeacherId = user1.Id,
                Teacher = user1
            };

            Assignments assignment2 = new Assignments
            {
                Name = "Assignment PasswordChecker",
                Exercise = exercise2,
                ExerciseId = exercise2.Id,
                DateDue = new DateTime(2024, 12, 1),
                TeacherId = user1.Id,
                Teacher = user1
            };

            var assignmentUsers = new List<AssignmentUser>
            {
                new AssignmentUser { Assignment = assignment1, Student = user2 },
                new AssignmentUser { Assignment = assignment1, Student = user3 },
                new AssignmentUser { Assignment = assignment1, Student = user4 },
                new AssignmentUser { Assignment = assignment1, Student = user5 },
                new AssignmentUser { Assignment = assignment1, Student = user6 },

                new AssignmentUser { Assignment = assignment2, Student = user2 },
                new AssignmentUser { Assignment = assignment2, Student = user3 },
                new AssignmentUser { Assignment = assignment2, Student = user4 },
            };

            var users = new List<Student> { user2, user3, user4, user5, user6 };
            var teachers = new List<Teacher> { user1 };

            var tags = new List<Tag> { tag1, tag2, tag3, tag4, tag5, tag6 };

            var exercises = new List<Exercise> { exercise1, exercise2, exercise3, exercise4, exercise5, exercise6, exercise7, exercise8, exercise9, exercise10, exercise11 };



            return new ImportData
            {
                AssignmentUsers = assignmentUsers,
                Tags = tags,
                Students = users,
                Teachers = teachers,
                Exercises = exercises,
                Assignments = new List<Assignments> { assignment1, assignment2 },
                ArrayOfSnippets = new List<ArrayOfSnippets> { exercise1.ArrayOfSnippets, exercise2.ArrayOfSnippets, exercise3.ArrayOfSnippets, exercise4.ArrayOfSnippets, exercise5.ArrayOfSnippets, exercise6.ArrayOfSnippets, exercise7.ArrayOfSnippets, exercise8.ArrayOfSnippets, exercise9.ArrayOfSnippets, exercise10.ArrayOfSnippets, exercise11.ArrayOfSnippets },
                Snippets = new List<Snippet> { exercise1.ArrayOfSnippets.Snippets[0], exercise1.ArrayOfSnippets.Snippets[1], exercise1.ArrayOfSnippets.Snippets[2],
                    exercise2.ArrayOfSnippets.Snippets[0], exercise2.ArrayOfSnippets.Snippets[1], exercise2.ArrayOfSnippets.Snippets[2],
                    exercise3.ArrayOfSnippets.Snippets[0], exercise3.ArrayOfSnippets.Snippets[1], exercise3.ArrayOfSnippets.Snippets[2],
                    exercise4.ArrayOfSnippets.Snippets[0], exercise4.ArrayOfSnippets.Snippets[1], exercise4.ArrayOfSnippets.Snippets[2],
                    exercise5.ArrayOfSnippets.Snippets[0], exercise5.ArrayOfSnippets.Snippets[1], exercise5.ArrayOfSnippets.Snippets[2],
                    exercise6.ArrayOfSnippets.Snippets[0], exercise6.ArrayOfSnippets.Snippets[1], exercise6.ArrayOfSnippets.Snippets[2],
                    exercise7.ArrayOfSnippets.Snippets[0], exercise7.ArrayOfSnippets.Snippets[1], exercise7.ArrayOfSnippets.Snippets[2],
                    exercise8.ArrayOfSnippets.Snippets[0], exercise8.ArrayOfSnippets.Snippets[1], exercise8.ArrayOfSnippets.Snippets[2],
                    exercise9.ArrayOfSnippets.Snippets[0], exercise9.ArrayOfSnippets.Snippets[1], exercise9.ArrayOfSnippets.Snippets[2],
                    exercise10.ArrayOfSnippets.Snippets[0], exercise10.ArrayOfSnippets.Snippets[1], exercise10.ArrayOfSnippets.Snippets[2],
                    exercise11.ArrayOfSnippets.Snippets[0], exercise11.ArrayOfSnippets.Snippets[1], exercise11.ArrayOfSnippets.Snippets[2]
                }
            };
        }
    }
}
