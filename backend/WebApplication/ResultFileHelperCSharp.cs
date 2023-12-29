using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace LeoCodeBackend
{
    public class ResultFileHelperCSharp
    {
        public void ConvertTrxToJson()
        {
            string trxFilePath = "C:\\Schule\\4AHIF\\LeoCode\\backend\\languages\\CSharp\\PasswordTest\\results\\_0d32bfe019ae_2023-12-03_19_07_36.trx";
            string jsonFilePath = "C:\\test.json";

            try
            {
                var testResults = LoadTestResults(trxFilePath);

                Console.WriteLine($"Total Tests: {testResults.Count}");
                Console.WriteLine($"Passed Tests: {testResults.Count(result => result.Outcome == "Passed")}");
                Console.WriteLine($"Failed Tests: {testResults.Count(result => result.Outcome == "Failed")}");

                string jsonResult = ConvertTestResultsToJson(testResults);

                File.WriteAllText(jsonFilePath, jsonResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static List<TestResult> LoadTestResults(string trxFilePath)
        {
            var testResults = new List<TestResult>();

            XDocument trxDocument = XDocument.Load(trxFilePath);

            XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

            foreach (var testResultElement in trxDocument.Descendants(ns + "UnitTestResult"))
            {
                var testResult = new TestResult
                {
                    TestName = testResultElement.Attribute("testName").Value,
                    Outcome = testResultElement.Attribute("outcome").Value,
                    ErrorMessage = testResultElement.Attribute("errorMessage")?.Value
                };

                testResults.Add(testResult);
            }

            return testResults;
        }

        static string ConvertTestResultsToJson(List<TestResult> testResults)
        {
            Summary summary = new Summary()
            {
                TotalTests = testResults.Count,
                PassedTests = testResults.Count(result => result.Outcome == "Passed"),
                FailedTests = testResults.Count(result => result.Outcome == "Failed")
            };

            var jsonResults = new
            {
                Summary = summary,
                TestResults = testResults.Select(result =>
                    new TestResult
                    {
                        TestName = result.TestName,
                        Outcome = result.Outcome,
                        ErrorMessage = result.ErrorMessage
                    })
            };

            return JsonConvert.SerializeObject(jsonResults, Newtonsoft.Json.Formatting.Indented);
        }
    }
    public class TestResult
    {
        public string TestName { get; set; }
        public string Outcome { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Summary
    {
        public int TotalTests { get; set; }
        public int PassedTests { get; set; }
        public int FailedTests { get; set; }
    }

    public class CustomResults
    {
        public Summary Summary { get; set; }
        public List<TestResult> TestResults { get; set; }
    }
}
