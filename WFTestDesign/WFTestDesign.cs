using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Activities.XamlIntegration;



namespace WFTestDesign
{
    public class WFTestDesignInvoker
    {
        #region Declaration
        private static Activity m_Workflow;
        #endregion

        public WFTestDesignInvoker()
        {}


        public WFTestDesignInvoker(string xamlfile)
        {
            m_Workflow = GetWorkflow(xamlfile);
        }
        /// <summary>
        /// Retourne le workflow représenté par la string contenant le xaml
        /// </summary>
        /// <param name="xamlfile"></param>
        /// <returns></returns>
        private static Activity GetWorkflow(String xamlfile)
        {
            if (!String.IsNullOrEmpty(xamlfile))
            {
                // Création du XmlReader destiné à permettre la restitution du Workflow
                //StringReader stringReader = new StringReader(xamlfile);
                //XmlReader xmlReader = XmlReader.Create(stringReader);

                try
                {
                    // Création du workflow
                    //return ActivityXamlServices.Load(xmlReader);
                    return ActivityXamlServices.Load(xamlfile);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }


        public void LaunchTest()
        {
            WorkflowInvoker.Invoke(m_Workflow);
        }
    }
}
