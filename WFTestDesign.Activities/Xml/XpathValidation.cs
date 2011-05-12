using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Xml;
using System.ComponentModel;
using WFTestDesign.Activities.Helpers;

namespace WFTestDesign.Activities.Xml
{
    
    [Designer(typeof(WFTestDesign.Activities.Designers.Xml.XmlValidation))]
    public sealed class XpathValidation : CodeActivity
    {
        // Define an activity input argument of type string
        [DisplayName("Xpath Expression")]
        [Category("XmlValidation Property")]
        public InArgument<string> xpathexpression { get; set; }
        
        [DisplayName("Expected Value")]
        [Category("XmlValidation Property")]
        [Description("Expected value of the Xpath expression, could be any variable or constant of String Type")]
        public InArgument<string> expectedValue { get; set; }
        
        [Browsable(false)]
        public XmlDocument xmlDoc { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argume

            string xpathExp = context.GetValue(this.xpathexpression);
            string expectedValue = context.GetValue(this.expectedValue);

            
            Logger.TestStepDetail("XmlValidationStep evaluting XPath {0} equals \"{1}\"", xpathExp, expectedValue);


            XmlNode checkNode = xmlDoc.SelectSingleNode(xpathExp);

            if (0 != expectedValue.CompareTo(checkNode.InnerText))
            {
                
                throw new ApplicationException(string.Format("XmlValidationStep failed, compare {0} != {1}, xpath query used: {2}", expectedValue, checkNode.InnerText, xpathExp));
            }
            else
                Logger.TestStepDetail("XmlValidationStep succeed, compare {0} == {1}, xpath query used: {2}", expectedValue, checkNode.InnerText, xpathExp);


        }


    }
}
