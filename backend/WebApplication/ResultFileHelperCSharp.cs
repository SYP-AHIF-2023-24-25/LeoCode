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

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: TRX file not found. {ex.Message}");
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Error: XML parsing issue. {ex.Message}");
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
                    ErrorMessage = testResultElement.Attribute("errorMessage")?.Value,
                    ErrorStackTrace = testResultElement.Attribute("errorStackTrace")?.Value,
                    StartTime = DateTime.ParseExact(testResultElement.Attribute("startTime").Value, "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", null),
                    EndTime = DateTime.ParseExact(testResultElement.Attribute("endTime").Value, "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", null)
                };

                testResults.Add(testResult);
            }

            return testResults;
        }

        static string ConvertTestResultsToJson(List<TestResult> testResults)
        {
            var summary = new
            {
                TotalTests = testResults.Count,
                PassedTests = testResults.Count(result => result.Outcome == "Passed"),
                FailedTests = testResults.Count(result => result.Outcome == "Failed")
            };

            var jsonResults = new
            {
                Summary = summary,
                TestResults = testResults.Select(result =>
                    new
                    {
                        TestName = result.TestName,
                        Outcome = result.Outcome,
                        ErrorMessage = result.ErrorMessage,
                        ErrorStackTrace = result.ErrorStackTrace,
                        StartTime = result.StartTime,
                        EndTime = result.EndTime
                    })
            };

            return JsonConvert.SerializeObject(jsonResults, Newtonsoft.Json.Formatting.Indented);
        }
    }
    class TestResult
    {
        public string TestName { get; set; }
        public string Outcome { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStackTrace { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
