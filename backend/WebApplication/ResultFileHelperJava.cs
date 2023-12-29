using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;

namespace LeoCodeBackend
{
    public class ResultFileHelperJava
    {
        public void ConvertXmlToJson()
        {
            string xmlFilePath = "C:\\Schule\\4AHIF\\LeoCode\\backend\\languages\\Java\\AppleTest\\results\\TEST-org.example.AppTest.xml";
            string jsonFilePath = "C:\\Users\\Florian\\Desktop\\Converter_Java\\test.json";

            try
            {
                var testResults = LoadTestResultsFromXml(xmlFilePath);

                Console.WriteLine($"Total Tests: {testResults.Count}");
                Console.WriteLine($"Passed Tests: {testResults.Count(result => result.Outcome == "Passed")}");
                Console.WriteLine($"Failed Tests: {testResults.Count(result => result.Outcome == "Failed")}");

                string jsonResult = ConvertTestResultsToJson(testResults);

                File.WriteAllText(jsonFilePath, jsonResult);

                Console.WriteLine("Konvertierung erfolgreich abgeschlossen.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Fehler: XML-Datei nicht gefunden. {ex.Message}");
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Fehler: XML-Parsing-Problem. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }
        }
        static List<TestResult> LoadTestResultsFromXml(string xmlFilePath)
        {
            var testResults = new List<TestResult>();

            XDocument xmlDocument = XDocument.Load(xmlFilePath);

            XNamespace ns = xmlDocument.Root.GetNamespaceOfPrefix("xsi");

            foreach (var testCaseElement in xmlDocument.Descendants("testcase"))
            {
                string outcome = "Passed";
                if (testCaseElement.Value != "")
                {
                    outcome = "Failed";
                }
                TestResult testResult = new TestResult()
                {
                    TestName = testCaseElement.Attribute("name").Value,
                    Outcome = outcome,
                    ErrorMessage = testCaseElement.Value
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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
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

