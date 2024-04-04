using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

            XElement xml = XElement.Parse(xmlData);

            foreach (var testResult in xml.Descendants("{http://microsoft.com/schemas/VisualStudio/TeamTest/2010}UnitTestResult"))
            {
                string testName = testResult.Attribute("testName")?.Value;
                string outcome = testResult.Attribute("outcome")?.Value;
                string errorMessage = "";

                if (outcome == "Failed")
                {
                    errorMessage = testResult.Descendants("Message").FirstOrDefault()?.Value ?? "";
                }

                // Debug output
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

                var result = new Dictionary<string, string>
                {
                    { "TestName", testName },
                    { "Outcome", outcome },
                    { "ErrorMessage", errorMessage }
                };

                testResults.Add(result);
            }

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
