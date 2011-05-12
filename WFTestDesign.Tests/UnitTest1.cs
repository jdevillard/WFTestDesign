using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace WFTestDesign.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
           // WorkflowInvoker.Invoke(new Workflow2xaml());
            WFTestDesignInvoker bizunit = new WFTestDesignInvoker(@"E:\Dev\VS2010\WFTestDesign\WFTestDesign.ConsoleWorkflow\Workflow2.xaml");
            bizunit.LaunchTest();
        }
    }
}
