using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WFTestDesign.Tests
{
    [TestClass]
    public class UnitTest3
    {
       

        [TestMethod]
        public void TestDBQuery()
        {
            // WorkflowInvoker.Invoke(new Workflow2xaml());
            WFTestDesignInvoker bizunit = new WFTestDesignInvoker(@"D:\Dev\CodePlex\wftestdesign\WFTestDesign.Tests\TestDBQuery.xaml");
            bizunit.LaunchTest();
        }
    }
}
