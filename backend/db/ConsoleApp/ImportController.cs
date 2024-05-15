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
                Username = "IF200183",
                Password = "123456"
            };
            User user2 = new User
            {
                Username = "IF200190",
                Password = "123456"
            };

            var users = new List<User> { user1, user2 };

            Exercise exercise1 = new Exercise
            {
                Name = "BubbleSort",
                Description = "Implement the BubbleSort algorithm",
                Language = Language.CSharp,
                Project = new byte[] { 0x00, 0x01, 0x02, 0x03 },
                Year = Year.First,
                Subject = Subject.WMC
            };

            Exercise exercise2 = new Exercise
            {
                Name = "QuickSort",
                Description = "Implement the QuickSort algorithm",
                Language = Language.Java,
                Project = new byte[] { 0x00, 0x01, 0x02, 0x03 },
                Year = Year.Second,
                Subject = Subject.POSE
            };

            var exercises = new List<Exercise> { exercise1, exercise2 };

            var snippets1 = new List<Snippet>
            {
                new Snippet
                {
                    Code = "public class Addition{public static void Main(){}public static int AdditionCalculation(int firstNumber, int secondNumber){",
                    ReadonlySection = true,
                    FileName = "Program.cs",
                    User = user1,
                    Exercise = exercise1,
                },
                new Snippet
                {
                    Code = "return firstNumber - secondNumber;",
                    ReadonlySection = false,
                    FileName = "Program.cs",
                    User = user1,
                    Exercise = exercise1,
                },
                new Snippet
                {
                    Code = "}}",
                    ReadonlySection = true,
                    FileName = "Program.cs",
                    User = user1,
                    Exercise = exercise1,
                }
            };

            var snippets2 = new List<Snippet>
            {
                new Snippet
                {
                    Code = "public class FalseOrTrueGenerator{public static void Main(){}public static bool FalseOrTrueGenerator(){",
                    ReadonlySection = true,
                    FileName = "Program.cs",
                    User = user2,
                    Exercise = exercise2,
                },
                new Snippet
                {
                    Code = "return true;",
                    ReadonlySection = false,
                    FileName = "Program.cs",
                    User = user2,
                    Exercise = exercise2,
                },
                new Snippet
                {
                    Code = "}}",
                    ReadonlySection = true,
                    FileName = "Program.cs",
                    User = user2,
                    Exercise = exercise2,
                }
            };

            var snippets = snippets1.Concat(snippets2).ToList();

            var arrayOfSnippets1 = new ArrayOfSnippets
            {
                Snippets = snippets1,
                User = user1,
            };

            var arrayOfSnippets2 = new ArrayOfSnippets
            {
                Snippets = snippets2,
                User = user2,
            };

            var arrayOfSnippets = new List<ArrayOfSnippets> { arrayOfSnippets1, arrayOfSnippets2 };

            return new ImportData
            {
                Exercises = exercises,
                Users = users,
                Snippets = snippets,
                ArrayOfSnippets = arrayOfSnippets
            };
        }
    }
}
