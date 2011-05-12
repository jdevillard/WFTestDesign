using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;



namespace WFTestDesign.Activities.File
{


    [ToolboxBitmap(typeof(WFTestDesign.Activities.File.FileMove), "Icon.Move.bmp")] 
    public sealed class FileMove : CodeActivity
    {
        #region Declarations

        [Category("FileCreate Property")]
        [DisplayName("Source File")]
        [Description("Source File Path which need to be copied, indicated the full path of the file")]
        public string sourcePath { get; set; }

        [Category("FileCreate Property")]
        [DisplayName("Destination File")]
        [Description("Target File Path, indicated the full path of the file using WildCard if necessary ")]
        public string targetPath { get; set; }
        #endregion



        #region Implementation
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            Execute();
        }

        public void Execute()
        {

            try
            {
                System.IO.File.Move(sourcePath, targetPath);
                Helpers.Logger.TestStepDetail("File successfully moved from {0} to {1}", sourcePath, targetPath);
            }

            finally
            {
              
            }
        }

        #endregion

        #region Metadata
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }


        static FileMove()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(FileMove),
                "sourcePath",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.FilePickerDialog)));

            builder.AddCustomAttributes(
                typeof(FileMove),
                "targetPath",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.FileSaverDialog)));


            MetadataStore.AddAttributeTable(builder.CreateTable());
       
        }
        #endregion
    }
}
