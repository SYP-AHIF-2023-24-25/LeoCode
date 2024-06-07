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
                Username = "Default",
                Password = "Default"
            };

            User user2 = new User
            {
                Username = "IF200182",
                Password = "123"
            };

            Exercise exercise1 = new Exercise
            {
                Name = "BubbleSort",
                Description = "Implement the BubbleSort algorithm",
                Language = Language.CSharp,
                Year = Year.First,
                Subject = Subject.CSharp,
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
                                Code = "public class BubbleSort{public static void Main(){}public static int[] BubbleSort(int[] array){",
                                ReadonlySection = true,
                                FileName = "Program.cs",
                                ArrayOfSnippets = exercise1.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise1.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "return array;",
                                ReadonlySection = false,
                                FileName = "Program.cs",
                                ArrayOfSnippets = exercise1.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise1.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "}",
                                ReadonlySection = true,
                                FileName = "Program.cs",
                                ArrayOfSnippets = exercise1.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise1.ArrayOfSnippets.Id
                            }
                        };

            Exercise exercise2 = new Exercise
            {
                Name = "QuickSort",
                Description = "Implement the QuickSort algorithm",
                Language = Language.Java,
                Year = Year.Second,
                Subject = Subject.POSE,
                
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
                                Code = "public class QuickSort{public static void Main(){}public static int[] QuickSort(int[] array){",
                                ReadonlySection = true,
                                FileName = "App.java",
                                ArrayOfSnippets = exercise2.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise2.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "return array;",
                                ReadonlySection = false,
                                FileName = "App.java",
                                ArrayOfSnippets = exercise2.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise2.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "}",
                                ReadonlySection = true,
                                FileName = "App.java",
                                ArrayOfSnippets = exercise2.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise2.ArrayOfSnippets.Id
                            }
                        };



            exercise1.UserId = user1.Id;
            exercise2.UserId = user1.Id;
            exercise1.User = user1;
            exercise2.User = user1;

            

            Exercise exercise3 = new Exercise
            {
                Name = "BubbleSort",
                Description = "Implement the BubbleSort algorithm",
                Language = Language.CSharp,
                Year = Year.First,
                Subject = Subject.CSharp,
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
                                Code = "public class BubbleSort{public static void Main(){}public static int[] BubbleSort(int[] array){",
                                ReadonlySection = true,
                                FileName = "Program.cs",
                                ArrayOfSnippets = exercise3.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise3.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "return array;",
                                ReadonlySection = false,
                                FileName = "Program.cs",
                                ArrayOfSnippets = exercise3.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise3.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "}",
                                ReadonlySection = true,
                                FileName = "Program.cs",
                                ArrayOfSnippets = exercise3.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise3.ArrayOfSnippets.Id
                            }
                        };

            Exercise exercise4 = new Exercise
            {
                Name = "QuickSort",
                Description = "Implement the QuickSort algorithm",
                Language = Language.Java,
                Year = Year.Second,
                Subject = Subject.POSE,

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
                                Code = "public class QuickSort{public static void Main(){}public static int[] QuickSort(int[] array){",
                                ReadonlySection = true,
                                FileName = "App.java",
                                ArrayOfSnippets = exercise4.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise4.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "return array;",
                                ReadonlySection = false,
                                FileName = "App.java",
                                ArrayOfSnippets = exercise4.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise4.ArrayOfSnippets.Id
                            },
                            new Snippet
                            {
                                Code = "}",
                                ReadonlySection = true,
                                FileName = "App.java",
                                ArrayOfSnippets = exercise4.ArrayOfSnippets,
                                ArrayOfSnippetsId = exercise4.ArrayOfSnippets.Id
                            }
                        };

            exercise3.UserId = user2.Id;
            exercise4.UserId = user2.Id;
            exercise3.User = user2;
            exercise4.User = user2;

            /*exercise1.User = user1;
            exercise2.User = user1;
            exercise1.UserId = user1.Id;
            exercise2.UserId = user1.Id;*/

            var exercises = new List<Exercise> { exercise1, exercise2, exercise3, exercise4 };

            var users = new List<User> { user1, user2 };
            return new ImportData
            {
                Exercises = exercises,
                Users = users,
                ArrayOfSnippets = new List<ArrayOfSnippets> { exercise1.ArrayOfSnippets, exercise2.ArrayOfSnippets, exercise3.ArrayOfSnippets, exercise4.ArrayOfSnippets },
                Snippets = new List<Snippet> { exercise1.ArrayOfSnippets.Snippets[0], exercise1.ArrayOfSnippets.Snippets[1], exercise1.ArrayOfSnippets.Snippets[2], exercise2.ArrayOfSnippets.Snippets[0], exercise2.ArrayOfSnippets.Snippets[1], exercise2.ArrayOfSnippets.Snippets[2], exercise3.ArrayOfSnippets.Snippets[0], exercise3.ArrayOfSnippets.Snippets[1], exercise3.ArrayOfSnippets.Snippets[2], exercise4.ArrayOfSnippets.Snippets[0], exercise4.ArrayOfSnippets.Snippets[1], exercise4.ArrayOfSnippets.Snippets[2] }
            };
        }
    }
}
