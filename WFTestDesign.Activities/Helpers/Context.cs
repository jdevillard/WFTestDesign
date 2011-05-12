using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFTestDesign.Activities.Helpers
{
   
    static class Context
    {
        private  const string DATE_TIME = "%DateTime%";
        private  const string DATE_TIME_ISO8601 = "%DateTimeISO8601%";
        private  const string SERVER_NAME = "%ServerName%";
        private  const string GUID = "%Guid%";


        static public string SubstituteWildCards(string rawString)
        {
            //ArgumentValidation.CheckForNullReference(rawString, "rawString");

            string result = rawString;

            if (result.Contains(DATE_TIME))
            {
                result = result.Replace(DATE_TIME, DateTime.Now.ToString("HHmmss-ddMMyyyy"));
            }

            if (result.Contains(DATE_TIME_ISO8601))
            {
                result = result.Replace(DATE_TIME_ISO8601, DateTime.Now.ToString("s"));
            }

            if (result.Contains(SERVER_NAME))
            {
                result = result.Replace(SERVER_NAME, Environment.MachineName);
            }

            if (result.Contains(GUID))
            {
                result = result.Replace(GUID, Guid.NewGuid().ToString());
            }

            return result;
        }
    }
}
