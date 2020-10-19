using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TestReportGenerator
{
    public class Generate_Customized_Report : CodeActivity
    {

        // Finished Creation of End Test Activity
        private InArgument<String> statusValue = "PASSED";

        [Category("Status")]
        [RequiredArgument]
        public InArgument<String> Status
        {
            get { return this.statusValue; }
            set { this.statusValue = value; }
        }

        private static readonly Regex Regex = new Regex("[^a-zA-Z0-9 -]");


        protected override void Execute(CodeActivityContext context)
        {
            if (File.Exists("Temp.xml"))
            {

                XDocument doc = XDocument.Load("Temp.xml");

                XElement updateStartedElement = doc.Element("Test-Suite").Element("Result").Element("Status");
                updateStartedElement.Value = Status.Get(context);

                XElement addEndedElement = doc.Element("Test-Suite").Element("Result");
                addEndedElement.Add(new XElement("Ended", DateTime.Now.ToString()));

                string testScenario = doc.Element("Test-Suite").Element("Result").Element("TestScenario").Value;
                string testCase = doc.Element("Test-Suite").Element("Result").Element("TestName").Value;
                string startedTime = doc.Element("Test-Suite").Element("Result").Element("Started").Value;
                string endedTime = doc.Element("Test-Suite").Element("Result").Element("Ended").Value;
                string status = doc.Element("Test-Suite").Element("Result").Element("Status").Value;

                doc.Save("Temp.xml");

                // Create or Append to Testing-Report.XML
                if (!File.Exists("Testing-Report.xml"))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.NewLineOnAttributes = true;
                    using (XmlWriter xmlWriter = XmlWriter.Create("Testing-Report.xml", xmlWriterSettings))
                    {
                        xmlWriter.WriteStartDocument();
                        xmlWriter.WriteStartElement("Test-Suite");
                        xmlWriter.WriteStartElement("Result");
                        xmlWriter.WriteElementString("TestScenario", testScenario);
                        xmlWriter.WriteElementString("TestName", testCase);
                        xmlWriter.WriteElementString("Started", startedTime);
                        xmlWriter.WriteElementString("Ended", endedTime);
                        xmlWriter.WriteElementString("Status", status);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndDocument();
                        xmlWriter.Flush();
                        xmlWriter.Close();

                        XDocument doc_Main = XDocument.Load("Testing-Report.xml");
                        XElement element = doc_Main.Element("Test-Suite");

                        // Add Atrributes
                        CheckAndUpdateAttributes(element);

                        //Increment Total Attributes
                        // IncrementTotalANDPassedAttributes(element);

                        //Increment Total Attributes based on execution
                        if (status.ToLower().Equals("passed"))
                        {
                            UpdatePasedDetails(element);
                        }
                        else if (status.ToLower().Equals("failed"))
                        {
                            UpdateFailedDetails(element);
                        }

                        //Save Element
                        element.Save("Testing-Report.xml");
                    }
                }
                else
                {
                    XDocument doc_Main = XDocument.Load("Testing-Report.xml");
                    XElement element = doc_Main.Element("Test-Suite");

                    // Increment Passed & Failures
                    // IncrementTotalANDPassedAttributes(element);

                    //Increment Total Attributes based on execution
                    if (status.ToLower().Equals("passed"))
                    {
                        UpdatePasedDetails(element);
                    }
                    else if (status.ToLower().Equals("failed"))
                    {
                        UpdateFailedDetails(element);
                    }

                    //Save element
                    element.Save("Testing-Report.xml");

                    //Add Element
                    element.Add(new XElement("Result", new XElement("TestScenario", testScenario),
                        new XElement("TestName", testCase),
                        new XElement("Started", startedTime),
                        new XElement("Ended", endedTime),
                        new XElement("Status", status)));
                    element.Save("Testing-Report.xml");
                }

                //Update "total-time" attribute
                UpdateTotalTime(DateTime.Parse(startedTime), DateTime.Parse(endedTime));

                //Delete Temp.xml since its of no use now
                File.Delete("Temp.xml");

                // Start Processing HTML file from here..
                GenerateHTMLFile();

            }
            else
            {
                // Since file doesn't exist and it may have been already deleted only in case of failed status,
                // Increment "failures" attribute value
                XDocument doc_Main = XDocument.Load("Testing-Report.xml");
                XElement element = doc_Main.Element("Test-Suite");

                // IncrementTotalANDFailedAttributes(element);
                //Console.WriteLine("Temp.xml File doesn't exists. Hence test is Failed.");
                //Increment Total Attributes based on execution
          
                //Save element
                element.Save("Testing-Report.xml");

                Console.WriteLine("-----here i am--------");

                //Generate HTML Again
                GenerateHTMLFile();
            }
            // GenerateHTMLFile();
        }

        private static void UpdateTotalTime(DateTime startTime, DateTime endTime)
        {
            TimeSpan diff = endTime - startTime;

            XDocument doc = XDocument.Load("Testing-Report.xml");
            XElement element = doc.Element("Test-Suite");

            TimeSpan totalTime = TimeSpan.Parse(element.Attribute("total-time").Value) + diff;
            element.Attribute("total-time").Value = totalTime.ToString();
            element.Save("Testing-Report.xml");
        }

        private static void GenerateHTMLFile()
        {
            StringBuilder html = new StringBuilder();
            string input = "Testing-Report.xml", output = string.Empty;
            output = Path.ChangeExtension(input, "html");
            
            html.Append(generateReport.generateHeaderOnTestEnd(input));
            // Save HTML to the output file
            File.WriteAllText(output, html.ToString());
        }

        /*private static void IncrementTotalANDFailedAttributes(XElement element)
        {
            // Increment "total" attribute value
            int valTotal = Convert.ToInt32(element.Attribute("total").Value);

            // Update "failure" attribute value
            int valFailure = Convert.ToInt32(element.Attribute("failures").Value);
            valFailure = valFailure + 1;
            element.Attribute("failures").Value = valFailure.ToString();

            // Update "passed" attribute value
            int valPassed = valTotal - valFailure;
            element.Attribute("passed").Value = valPassed.ToString();

            //Save element
            element.Save("Testing-Report.xml");
        }*/

        /*private static void IncrementTotalANDPassedAttributes(XElement element)
        {
            // Increment "total" attribute value
            int valTotal = Convert.ToInt32(element.Attribute("total").Value);
            valTotal = valTotal + 1;
            element.Attribute("total").Value = valTotal.ToString();

            // Update "passed" attribute value
            int valPassed = Convert.ToInt32(element.Attribute("passed").Value);
            valPassed = valPassed + 1;
            element.Attribute("passed").Value = valPassed.ToString();

            // Update Failures
            int valFailures = valTotal - valPassed;
            element.Attribute("failures").Value = valFailures.ToString();

            //Save element
            element.Save("Testing-Report.xml");
        }*/

        private static void CheckAndUpdateAttributes(XElement element)
        {

            //if (element.Attribute("name") == null)
            {
                element.Add(new XAttribute("name", "Workflow Automation (ITE)"));
            }
            //if (element.Attribute("total") == null)
            {
                element.Add(new XAttribute("total", "0"));
            }

            //if (element.Attribute("passed") == null)
            {
                element.Add(new XAttribute("passed", "0"));
            }

            //if (element.Attribute("failures") == null)
            {
                element.Add(new XAttribute("failed", "0"));
            }

            //if (element.Attribute("execution-date") == null)
            {
                element.Add(new XAttribute("execution-date", DateTime.Now.ToString()));
            }

            //if (element.Attribute("total-time") == null)
            {
                element.Add(new XAttribute("total-time", "0"));
            }
        }

        private static void UpdatePasedDetails(XElement element)
        {
            // Increment "total" attribute value
            int valTotal = Convert.ToInt32(element.Attribute("total").Value);
            valTotal = valTotal + 1;
            element.Attribute("total").Value = valTotal.ToString();

            // Update "passed" attribute value
            int valPassed = Convert.ToInt32(element.Attribute("passed").Value);
            valPassed = valPassed + 1;
            element.Attribute("passed").Value = valPassed.ToString();
            Console.WriteLine("Passed--------" + valPassed);
            //Save element
            element.Save("Testing-Report.xml");
        }

        private static void UpdateFailedDetails(XElement element)
        {
            // Increment "total" attribute value
            int valTotal = Convert.ToInt32(element.Attribute("total").Value);
            valTotal = valTotal + 1;
            element.Attribute("total").Value = valTotal.ToString();

            // Update Failures
            //int valFailures = valTotal - valPassed;
            int valFailures = Convert.ToInt32(element.Attribute("failed").Value);
            valFailures = valFailures + 1;
            element.Attribute("failed").Value = valFailures.ToString();
            Console.WriteLine("Failed--------" + valFailures);

            //Save element
            element.Save("Testing-Report.xml");
        }




        private static string GetHTML5Footer()
        {
            StringBuilder footer = new StringBuilder();
            footer.AppendLine("  </body>");
            footer.AppendLine("</html>");

            return footer.ToString();
        }

    }
}