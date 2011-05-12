using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Drawing;
using WFTestDesign.Activities.Helpers;




namespace WFTestDesign.Activities.File
{
    using System.IO;
using System.ComponentModel;
    using System.Activities.Presentation.Metadata;
    using System.Activities.Presentation.PropertyEditing;


    [ToolboxBitmap(typeof(FileCreate), "Icon.Copy.bmp")] 
    [Designer(typeof(WFTestDesign.Activities.Designers.File.FileCreate))]
    public sealed class FileCreate : CodeActivity
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
        //private Context ctx;


        #endregion


        #region Implementation
        protected override void Execute(CodeActivityContext context)
        {
            //ctx = new Context();
            Execute();
        }

        public void Execute()
        {

           FileStream dstFs = null;
           FileStream srcFs = null;

            try
            {
                Logger.TestStepDetail("FileCreateStep about to copy the data from File: {0} to the File: {1}", sourcePath, targetPath);

                srcFs = File.Open(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                dstFs = File.Create(Context.SubstituteWildCards(targetPath));
                byte[] buff = new byte[4096];

                int read = srcFs.Read(buff, 0, 4096);

                while (read > 0)
                {
                    dstFs.Write(buff, 0, read);
                    read = srcFs.Read(buff, 0, 4096);
                }
            }
            finally
            {
                if (null != srcFs)
                {
                    srcFs.Close();
                }

                if (null != dstFs)
                {
                    dstFs.Close();
                }
            }
        }
        #endregion

        #region Metadata
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
        }

        static FileCreate()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(FileCreate),
                "sourcePath",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.FilePickerDialog)));

            builder.AddCustomAttributes(
                typeof(FileCreate),
                "targetPath",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.FileSaverDialog)));


            MetadataStore.AddAttributeTable(builder.CreateTable());
       
        }

        #endregion
    }
}
