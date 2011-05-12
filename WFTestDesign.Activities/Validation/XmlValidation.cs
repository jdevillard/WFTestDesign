using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Xml;
using WFTestDesign.Activities.Helpers;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel;
using Microsoft.XmlDiffPatch;

namespace WFTestDesign.Activities.Validation
{

    public sealed class XmlValidation : CodeActivity
    {
        #region Declarations
        // Define an activity input argument of type string
        public InArgument<string> ValidateFilePath { get; set; }

        [Browsable(false)]
        public MemoryStream StreamToValidate {get;set;}

        #endregion

        #region Implementation
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            
            string ValidatorFilePath = context.GetValue(this.ValidateFilePath);
            MemoryStream data = StreamToValidate;

            try
            {
                XmlDocument doc1 = new XmlDocument();
                XmlDocument doc2 = new XmlDocument();
          
                doc1 = ReadFile(data);
                Logger.TestStepDetail("Message to validate loaded");

                doc2.Load(ValidatorFilePath);
                Logger.TestStepDetail("Message validator loaded");
                
               
                #region Suppression des noeuds qui varient à chaque nouveau test
                XmlNode xmlNodeDoc1 = doc1.SelectSingleNode("//*[local-name()='DateDebutTraitement']");
                if (xmlNodeDoc1 != null)
                    xmlNodeDoc1.ParentNode.RemoveChild(xmlNodeDoc1);

                XmlNode xmlNodeDoc2 = doc2.SelectSingleNode("//*[local-name()='DateDebutTraitement']");
                if (xmlNodeDoc2 != null)
                    xmlNodeDoc2.ParentNode.RemoveChild(xmlNodeDoc2);

                #endregion
                

                XmlDiff xmlDiff = new XmlDiff(XmlDiffOptions.IgnoreComments | XmlDiffOptions.IgnoreWhitespace | XmlDiffOptions.IgnoreChildOrder);
                XmlWriter diffgramWriter = new XmlTextWriter(new StreamWriter("diffgram.xml"));
                bool result = xmlDiff.Compare(doc1, doc2, diffgramWriter);
                diffgramWriter.Close();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("diffgram.xml");

                //Assert.IsTrue(result, xmlDoc.OuterXml);
                if (!result)
                    throw new ApplicationException("Erreur de validation XML : " + xmlDoc.OuterXml);
             
            }
            catch (Exception)
            {
                throw;
            }

        }
        private XmlDocument ReadFile(Stream xmlstream)
        {

            XmlDocument xmldoc = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(xmlstream))
            {

                xmldoc.Load(reader);
            }

            return xmldoc;
        }

        #endregion
    }
}
