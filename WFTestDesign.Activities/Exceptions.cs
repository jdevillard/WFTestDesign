using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFTestDesign.Activities
{
    public class WFTestExceptions : ApplicationException
    {
        public WFTestExceptions(string message):
            base(message)
        {}

        public WFTestExceptions(string message,params object[] args):
            base(string.Format(message,args))
        {
        }
    }
}
