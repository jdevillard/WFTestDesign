using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WFTestDesign;


namespace $safeprojectname$
{
    [TestClass]
    public class $safeitemrootname$Class
    {
        [TestMethod]
        public void TestMethod1()
        {
            WFTestDesignInvoker wfTest = new WFTestDesignInvoker(@"$destinationdirectory$\UnitTestLauncher.xaml");
            wfTest.LaunchTest();
        }
    }
}
