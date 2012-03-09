using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Win32;
using System.Diagnostics;

namespace WFTestDesign.WizardTemplates
{
    public class CustomParameterWizard : IWizard
    {
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
            Trace.Write(String.Format("CustomParameterWizard -- Entering Method : {0}", "BeforeOpeningFile"));
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
            Trace.Write(String.Format("CustomParameterWizard -- Entering Method : {0}", "ProjectFinishedGenerating"));

            
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            Trace.Write(String.Format("CustomParameterWizard -- Entering Method : {0}", "ProjectItemFinishedGenerating"));
        }

        public void RunFinished()
        {
            Trace.Write(String.Format("CustomParameterWizard -- Entering Method : {0}", "Runfinished"));
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Trace.Write(String.Format("CustomParameterWizard -- Entering Method : {0}", "RunStarted"));
            //add custom parameter 
            string installDirectory;
            try
            {
                installDirectory = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"Software\WFTestDesign\WFTestDesign").GetValue("installDir").ToString();
                if (!installDirectory.EndsWith(@"\"))
                    installDirectory += @"\";
            }
            catch (Exception)
            {

                installDirectory = string.Empty;
            }
             
            replacementsDictionary.Add("$InstallDirectoryFromRegistry$",installDirectory);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            Trace.Write(String.Format("CustomParameterWizard -- Entering Method : {0} , file : {1}", "ShouldAddProjectItem", filePath));
            return true;
        }
    }
}
