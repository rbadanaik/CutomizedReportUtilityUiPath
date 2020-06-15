using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestReportGenerator
{
    class generateReport
    {

       public static string generateHeaderOnTestEnd(string file)
        {
            StringBuilder html = new StringBuilder();
            XElement doc = XElement.Load(file);

            // Load summary values
            string scenarioName = doc.Attribute("name").Value;
            int testTests = int.Parse(!string.IsNullOrEmpty(doc.Attribute("total").Value) ? doc.Attribute("total").Value : "0");
            int testPassed = int.Parse(!string.IsNullOrEmpty(doc.Attribute("passed").Value) ? doc.Attribute("passed").Value : "0");
            int testFailures = int.Parse(!string.IsNullOrEmpty(doc.Attribute("failures").Value) ? doc.Attribute("failures").Value : "0");
            DateTime testDate = DateTime.Parse(string.Format("{0}", doc.Attribute("execution-date").Value));
            TimeSpan totalTime = TimeSpan.Parse(!string.IsNullOrEmpty(doc.Attribute("total-time").Value) ? doc.Attribute("total-time").Value : "0");
            //string testPlatform = doc.Element("environment").Attribute("platform").Value;

            // Calculate the success rate
            decimal percentage = 0;
            if (testTests > 0)
            {
                int failures = testFailures;
                // percentage = decimal.Round(decimal.Divide(failures, testTests) * 100, 1);
                percentage = decimal.Round(decimal.Divide(testPassed, testTests) * 100, 1);
            }

            string summaryRowBackgroundColor = "";

            if (percentage == 100 )
            {
                summaryRowBackgroundColor = PassedBackgroundColor();
            } else if (percentage <= 75)
            {
                summaryRowBackgroundColor = WarningBackgroundColor();
            }
            else
            {
                summaryRowBackgroundColor = FailedBackgroundColor();
            }

            // StringBuilder generateHeader = new StringBuilder();
            html.Append(ReportTemplate.HeaderTemplate);
            html.Append(ReportTemplate.ReportTitleAndSummaryTableTemplate);


            String summaryRow = ReportTemplate.SummaryRowTemplate
                    .Replace("%SUMMARYROWBACKGROUNDCOLOR%", summaryRowBackgroundColor)
                    .Replace("%FEATURENAME%", scenarioName.Replace("[^\\w\\s]", ""))
                    .Replace("%COUNTER%", testTests.ToString())
                    .Replace("%PASSED%", testPassed.ToString())
                    .Replace("%FAILED%", testFailures.ToString())
                    // .Replace("%SKIPPED%", String.valueOf(skippedScenarios))
                    .Replace("%TOTAL%", testTests.ToString())
                    .Replace("%PASSPERCENTAGE%", percentage.ToString());
            // .Replace("%FEATUREDESCRIPTION%", featureDescription.replaceAll("[^\\w\\s]", ""));

            html.Append(summaryRow);

            html.Append(ReportTemplate.TableClosingTags);

            // Add Feature table information

            string headerTemplate = ReportTemplate.FeatureTableHeaderTemplate
                    //.Replace("%PREINCREMENTCOUNTER%", String.valueOf(++tableCounter))
                    .Replace("%FEATURENAME%", scenarioName.Replace("[^\\w\\s]", ""));
                   // .Replace("%TIMETAKENFEATURE%", totalTime.ToString() + " )");
                   // .replace("%COUNTER%", String.valueOf(tableCounter));

            html.Append(headerTemplate);


            XDocument xmlDoc = XDocument.Load("Testing-Report.xml");
            //Console.WriteLine(xmlDoc.Descendants("Result"));


            foreach (XElement element in xmlDoc.Descendants("Result"))
            {
                string testScenario = element.Element("TestScenario").Value;
                string testcaseName = element.Element("TestName").Value;
                string startTime = element.Element("Started").Value;
                string endTime = element.Element("Ended").Value;
                string status = element.Element("Status").Value;
                string alertType = "";
                string timeTaken = getTimeTaken(startTime,endTime);
                string snapshotLink = "";

                if (status.ToUpper().Equals("PASSED"))
                {
                    alertType = string.Format(" %1$s", PassedBackgroundColor());
                }
                else
                {
                    string snapshotLinkPath = getSnapshotPath();
                    Console.WriteLine("Inside else block: "+snapshotLinkPath);
                    snapshotLink = string.Format("<a href=\"%1$s\" target='_blank'>Click Here</a>", snapshotLinkPath);
                    alertType = string.Format(" %1$s", FailedBackgroundColor());
                }

                String scenario = testScenario.Replace("[^\\w\\s]", "");
                String scenarioRow = ReportTemplate.ScenarioTemplate.Replace("%SCENARIONAME%", scenario)
                        .Replace("%ALERTTYPE%", alertType).Replace("%SCENARIOSTATUS%", status)
                        //.replace("%ALERTERRORMESSAGE%", alertErrorMessage).
                        .Replace("%SNAPSHOTLINK%", snapshotLink)
                        .Replace("%TIMETAKEN%", timeTaken)
                        .Replace("%STEPSTABLE%", scenario.Replace(" ", ""));
               
                html.Append(scenarioRow);
            }

            html.Append(ReportTemplate.ReportClosingTags);

            return html.ToString();


        }

        private static string getTimeTaken(string startTime,string endTime)
        {
            return (DateTime.Parse(endTime) -  DateTime.Parse(startTime)).ToString();
        }

        private static string FailedBackgroundColor()
        {
            return "alert alert-danger";
        }

        private static string PassedBackgroundColor()
        {
            return "alert alert-success";
        }

        private static string WarningBackgroundColor()
        {
            return "alert alert-warning";
        }

        private static string SkippedBackgroundColour()
        {
            return "bg-light";
        }

        private static string getSnapshotPath()
        {
            string curDir = Directory.GetCurrentDirectory();
            string screenshotDir = $@"{curDir}\.screenshots";
            DirectoryInfo directory = new DirectoryInfo(screenshotDir);
            var myFile = (from f in directory.GetFiles()
                          orderby f.LastWriteTime descending
                          select f).First();
            string screenshotName = myFile.FullName;

            Console.WriteLine("File Details: " + myFile);
            Console.WriteLine("Screenshot Name: " + screenshotName);

            return screenshotName;
        }

    }
}
