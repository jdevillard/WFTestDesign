using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Windows.Markup;
using System.IO;
using System.Threading;
using WFTestDesign.Activities.Helpers;
using System.Collections.ObjectModel;
using System.Xml;
using System.ComponentModel;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;

namespace WFTestDesign.Activities.File
{
    //[Designer(typeof(Designers.GenericSequenceDesigner))]
    [Designer(typeof(Designers.File.ValidateFile))]
    public sealed class ValidateFile : NativeActivity
    {
        // Define an activity input argument of type string
        #region Declarations
        private double timeout;
        private string directory;
        private string searchPattern;
        private bool deleteFile;
        public readonly Collection<Activity> activities;


        private List<Activity> xpathActivities;
        private List<Activity> binaryValidationActivities;
        private MemoryStream binaryValidationMemoryStream;


        private CompletionCallback m_CompletionCallback;
        private FaultCallback m_FaultCallback;
        private int m_Index;
        private XmlDocument xmldoc;
        private string exceptionString;
        private Exception validationexception;
        

        [Category("ValidateFile Property")]
        public double Timeout
        {
            set
            {
                this.timeout = value;
            }
            get
            {
                return this.timeout;
            }
        }

        [Category("ValidateFile Property")]
        [DependsOn("Timeout")]
        public string Directory
        {
            set
            {
                this.directory = value;
            }
            get
            {
                return this.directory;
            }
        }

        [Category("ValidateFile Property")]
        [DependsOn("Directory")]
        public string SearchPattern
        {
            set
            {
                this.searchPattern = value;
            }
            get
            {
                return this.searchPattern;
            }
        }

        [Category("ValidateFile Property")]
        [DependsOn("DeleteFile")]
        public bool DeleteFile
        {
            set
            {
                this.deleteFile = value;
            }
            get
            {
                return this.deleteFile;
            }
        }


        [Category("ValidateFile Property")]
        [Browsable(false)]
        [DependsOn("DeleteFile")]
        public Collection<Activity> Activities
        {
            get { return this.activities; }
        }

         public ValidateFile()
        {
            this.activities = new Collection<Activity>();
        }


        #endregion

        #region Execution
         protected override void Execute(NativeActivityContext context)
         {
            
             // Initialisation
             this.m_Index = 0;


             // Caching callback delegates
             this.m_CompletionCallback = new CompletionCallback(this.MyCompletionCallback);
             this.m_FaultCallback = new FaultCallback(this.MyFaultCallback);

             MemoryStream data = null;

             try
             {//TODO : verify \ at the end of directory properties
                 Logger.TestStepDetail("Searching for files in: \"{0}{1}\"", directory, searchPattern);

                 DateTime endTime = DateTime.Now + TimeSpan.FromMilliseconds(timeout);
                 FileInfo[] files;

                 do
                 {
                     DirectoryInfo di = new DirectoryInfo(directory);
                     files = di.GetFiles(searchPattern);

                     Thread.Sleep(100);
                 } while ((files.Length == 0) && (endTime > DateTime.Now));

                 if (files.Length == 0)
                 {
                     throw new ApplicationException(string.Format("No files were found at: {0}{1}", directory, searchPattern));
                 }

                 Logger.TestStepDetail("{0} files were found at : \"{1}{2}\"", files.Length, directory, searchPattern);

                 IOException ex = null;
                 do
                 {
                     try
                     {
                         using (FileStream fs = new FileStream(files[0].FullName, FileMode.Open, FileAccess.Read))
                         {
                             data = StreamHelper.LoadMemoryStream(fs);
                         }
                     }
                     catch (IOException ioex)
                     {
                         //   Logger.LogWarning("IOException caught trying to load file, will re-try if within timeout");
                         ex = ioex;
                         Thread.Sleep(100);
                     }
                 } while ((null == data) && (endTime > DateTime.Now));

                 if (null != ex)
                 {
                     throw ex;
                 }

                 /* If XpathValidationStep
                  * then we can loop while XpathValidation Activities are passed
                  */

                 xpathActivities = activities
                     .Where(a => a.GetType() == typeof(WFTestDesign.Activities.Xml.XpathValidation)).ToList();

                 binaryValidationActivities = activities
                     .Where(a => a.GetType() == typeof(WFTestDesign.Activities.Validation.BinaryValidationStep)
                    || a.GetType() == typeof(WFTestDesign.Activities.Validation.XmlValidation) ).ToList();


                 binaryValidationMemoryStream = data;
                 /*
                 xmldoc = ReadFile(data);

                 data.Seek(0, SeekOrigin.Begin);
                  * */

                 if (xpathActivities.Count != 0)
                 {
                     xmldoc = ReadFile(data);
                     data.Seek(0, SeekOrigin.Begin);
                     ScheduleNextXmlValidation(context);
                 }
                 if (binaryValidationActivities.Count != 0) 
                    ScheduleNextBinaryValidation(context);



                 if (deleteFile)
                 {
                     System.IO.File.Delete(files[0].FullName);
                 }
             }
             finally
             {
                 
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

         public void ScheduleNextBinaryValidation(NativeActivityContext context)
         {
             if (this.m_Index < this.binaryValidationActivities.Count)
             {
                 //log
                 Activity activity;

                 if (this.binaryValidationActivities[this.m_Index].GetType() == typeof(WFTestDesign.Activities.Validation.BinaryValidationStep))
                 {
                     WFTestDesign.Activities.Validation.BinaryValidationStep activity_ = (WFTestDesign.Activities.Validation.BinaryValidationStep)this.binaryValidationActivities[this.m_Index];
                     activity_.StreamToValidate = binaryValidationMemoryStream;
                     activity = activity_;
                 }
                 else
                 {
                     WFTestDesign.Activities.Validation.XmlValidation activity_ = (WFTestDesign.Activities.Validation.XmlValidation)this.binaryValidationActivities[this.m_Index];
                     activity_.StreamToValidate = binaryValidationMemoryStream;
                     activity = activity_;
                 }


                 
                 


                 context.ScheduleActivity(
                     activity,
                     MyBinaryCompletionCallback,
                     MyBinaryFaultCallback);

                 this.m_Index++;
             }
         }


         public void MyBinaryCompletionCallback(NativeActivityContext context, ActivityInstance completedInstance)
         {
             if (this.m_Index < this.binaryValidationActivities.Count)
                 ScheduleNextBinaryValidation(context);
             else
             {
                 if (!string.IsNullOrEmpty(exceptionString))
                     throw new Exception(exceptionString);

                 if (null != binaryValidationMemoryStream)
                 {
                     binaryValidationMemoryStream.Close();
                 }
             }
         }

         public void MyBinaryFaultCallback(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
         {
             //Log
             Logger.LogException(propagatedException);

             //Prepare for resume
             exceptionString += Environment.NewLine + propagatedException.Message;


             //Set that the fault was catched
             faultContext.HandleFault();
             faultContext.CancelChild(propagatedFrom);


             //log
             if (this.m_Index < this.binaryValidationActivities.Count)
             {
                 ScheduleNextBinaryValidation(faultContext);
             }
             else
             {
                 if (!string.IsNullOrEmpty(exceptionString))
                     throw new Exception(exceptionString);
             }
         }
       

         public void ScheduleNextXmlValidation(NativeActivityContext context)
         {
             if (this.m_Index < this.xpathActivities.Count)
             {
                 //log

                 WFTestDesign.Activities.Xml.XpathValidation activity = (WFTestDesign.Activities.Xml.XpathValidation)this.xpathActivities[this.m_Index];


                 
                 activity.xmlDoc = xmldoc;


                 context.ScheduleActivity(
                     activity,
                     MyCompletionCallback,
                     MyFaultCallback);

                 this.m_Index++;
             }

         }

         public void MyCompletionCallback(NativeActivityContext context, ActivityInstance completedInstance)
         {
             if (this.m_Index < this.xpathActivities.Count)
                 ScheduleNextXmlValidation(context);
             else
             {
                 if (!string.IsNullOrEmpty(exceptionString))
                     throw new Exception(exceptionString);
             }
         }

         public void MyFaultCallback(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
         {
             //Log
             Logger.LogException(propagatedException);

             //Prepare for resume
             exceptionString += Environment.NewLine + propagatedException.Message;


             //Set that the fault was catched
             faultContext.HandleFault();
             faultContext.CancelChild(propagatedFrom);


             //log
             if (this.m_Index < this.xpathActivities.Count)
             {
                 ScheduleNextXmlValidation(faultContext);
             }
             else
             {
                 if (!string.IsNullOrEmpty(exceptionString))
                     throw new Exception(exceptionString);
             }
         }
         #endregion

        #region Metadata
         protected override void CacheMetadata(NativeActivityMetadata metadata)
         {
             if (this.activities != null && this.activities.Count!=0)
             {
                 /*
                 if (this.activities.Count(a => a.GetType() != typeof(Xml.XmlValidation)) != 0)
                     metadata.AddValidationError("Cette activité ne supporte que des activités de type XmlValidation");
                 else
                    
                  * */
                    this.activities.ToList().ForEach(a => metadata.AddChild(a));
             }
         }

        static ValidateFile()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(ValidateFile),
                "Directory",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.DirectoryPickerDialog)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
       
        }
        #endregion



    }
}
