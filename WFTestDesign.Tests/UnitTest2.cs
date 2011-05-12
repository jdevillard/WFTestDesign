using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WFTestDesign.Tests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestActivite()
        {
            WFTestDesignInvoker bizunit = new WFTestDesignInvoker(@"E:\Dev\VS2010\WFTestDesign\WFTestDesign.Tests\Test1.xaml");
            bizunit.LaunchTest();
        }

        [TestMethod]
        public void TestEventLog()
        {
            WFTestDesignInvoker eventlogtest = new WFTestDesignInvoker(@"E:\Dev\VS2010\WFTestDesign\WFTestDesign.Tests\TestEventLog.xaml");
            eventlogtest.LaunchTest();
        }
    }
}
