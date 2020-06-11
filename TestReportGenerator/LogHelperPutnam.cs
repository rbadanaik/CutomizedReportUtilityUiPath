using System;
using System.Activities;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace TestReportGenerator
{
    public class LogHelperPutnam : CodeActivity
    {
        private static LogBase logger = null;

        private static readonly Regex Regex = new Regex("[^a-zA-Z0-9 -]");

        [Category("LogTarget")]
        [RequiredArgument]
        public InArgument<LogTarget> logTarget {get; set;}
        [Category("LogTarget")]
        public InArgument<String> logMessage { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            switch (logTarget.Get(context))
            {
                case LogTarget.File:
                    logger = new FileLogger();
                    logger.Log(logMessage.Get(context));
                    break;
                case LogTarget.EventLog:
                    logger = new EventLogger();
                    logger.Log(logMessage.Get(context));
                    break;
                default:
                    return;

            }
        }
    }
}
