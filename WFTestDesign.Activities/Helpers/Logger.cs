using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFTestDesign.Activities.Helpers
{
    public static class Logger
    {

        const string InfoLogLevel = "Info";
        const string ErrorLogLevel = "Error";
        const string WarningLogLevel = "Warning";
       
        //public Logger() { }




        public static void TestStepStart(string testStepName)
        {
            
             Console.WriteLine(
                    string.Format("Step: {0} started  @ {1}", testStepName,
                                  FormatDate(DateTime.Now)));
        }

        public static void TestStepEnd(string testStepName, DateTime time, Exception ex)
        {
            if (null == ex)
            {
                WriteLine(string.Format("Step: {0} ended @ {1}", testStepName, FormatDate(time)));
            }
            else
            {
                WriteLine(string.Format("Step: {0} ended @ {1} with ERRORS, exception: {2}", testStepName, FormatDate(time), ex.GetType().ToString()));
            }
        }

        public static void TestStageStart(String stage, DateTime time)
        {
            switch (stage)
            {
                case "Setup":
                    WriteLine(" ");
                    WriteLine("Setup Stage: started @ {0}", FormatDate(time));
                    break;

                case "Execution":
                    WriteLine(" ");
                    WriteLine("Execute Stage: started @ {0}", FormatDate(time));
                    break;

                case "CleanUp":
                    WriteLine(" ");
                    WriteLine("Cleanup Stage: started @ {0}", FormatDate(time));
                    break;
            }
        }

        private static string FormatDate(DateTime time)
        {
            return time.ToString("HH:mm:ss.fff dd/MM/yyyy");
        }

        public static void TestStepDetail(string s,params object[] args)
        {
            WriteLine(s, args);
        }

        private static void WriteLine(string s)
        {
         
                Console.WriteLine(s);
            
        }

        private static void WriteLine(string s, params object[] args)
        {
                Console.WriteLine(s, args);
            
        }

        public static void TestStageEnd(String stage, DateTime time, Exception stageException)
        {
            if (null != stageException)
            {
                LogException(stageException);
            }

            WriteLine(" ");
            if (null == stageException)
            {
                WriteLine("{0} Stage: ended @ {1}", stage, FormatDate(time));
            }
            else
            {
                WriteLine("{0} Stage: ended @ {1} with ERROR's", stage, FormatDate(time));
            }
        }

        public static void LogException(Exception e)
        {
            if (null == e)
            {
                return;
            }

            WriteLine(new string('*', 79));
            WriteLine("{0}: {1}", ErrorLogLevel, "Exception caught!");
            WriteLine(e.ToString());
            WriteLine(new string('*', 79));
        }
    }
}
