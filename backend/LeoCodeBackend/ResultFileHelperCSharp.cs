using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Net; // To use HtmlDecode

namespace LeoCodeBackend
{
    public class ResultFileHelperCSharp
    {
        public string formatXMLToJson(string responseBody)
        {
            // Deserialize the JSON string into a dynamic object
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);

            // Access the XML value from the dynamic object
            string xmlData = jsonResponse.value;

            var summary = new Dictionary<string, int>
            {
                { "TotalTests", 0 },
                { "PassedTests", 0 },
                { "FailedTests", 0 }
            };

            var testResults = new List<Dictionary<string, string>>();

            // Load the XML document and define the namespace
            XNamespace ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
            XElement xml = XElement.Parse(xmlData);

            // Iterate over all UnitTestResult elements
            foreach (var testResult in xml.Descendants(ns + "UnitTestResult"))
            {
                string testName = testResult.Attribute("testName")?.Value;
                string outcome = testResult.Attribute("outcome")?.Value;
                string errorMessage = "";

                // Check for failed tests
                if (outcome == "Failed")
                {
                    // Access ErrorInfo -> Message inside Output
                    var errorInfo = testResult.Element(ns + "Output")?.Element(ns + "ErrorInfo");
                    if (errorInfo != null)
                    {
                        // Extract and decode the error message
                        errorMessage = errorInfo.Element(ns + "Message")?.Value ?? "";
                        errorMessage = WebUtility.HtmlDecode(errorMessage);  // Decode HTML entities like &lt; and &gt;
                    }
                }

                // Debug output to ensure error message extraction works
                Console.WriteLine($"TestName: {testName}, Outcome: {outcome}, ErrorMessage: {errorMessage}");

                if (outcome == "Passed")
                {
                    summary["PassedTests"]++;
                }
                else if (outcome == "Failed")
                {
                    summary["FailedTests"]++;
                }

                summary["TotalTests"]++;

                // Add test results to the final JSON
                var result = new Dictionary<string, string>
                {
                    { "TestName", testName },
                    { "Outcome", outcome },
                    { "ErrorMessage", errorMessage }
                };

                testResults.Add(result);
            }

            // Prepare the final JSON output
            var jsonData = new Dictionary<string, object>
            {
                { "Summary", summary },
                { "TestResults", testResults }
            };

            string json = JsonConvert.SerializeObject(jsonData, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            return json;
        }
    }
}
