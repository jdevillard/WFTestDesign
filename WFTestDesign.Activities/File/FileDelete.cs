using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.IO;
using System.Windows.Markup;
using System.ComponentModel;
using System.Drawing;
using System.Activities.Presentation.Metadata;
using System.Activities.Presentation.PropertyEditing;

namespace WFTestDesign.Activities.File
{
    [ToolboxBitmap(typeof(WFTestDesign.Activities.File.FileDelete), "Icon.DeleteHS.bmp")] 
    [Designer(typeof(WFTestDesign.Activities.Designers.File.FileDelete))]
    public sealed class FileDelete : CodeActivity
    {

        #region Declarations

        [Category("FileDelete Property")]
        [DependsOn("DisplayName")]
        public string Directory { get; set; }

        [Category("FileDelete Property")]
        [DisplayName("Search Pattern")]
        [DependsOn("Directory")]
        public string SearchPattern {get;set;}
        
        
        #endregion


        #region Implementation
        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            if (!this.Directory.EndsWith(@"\"))
                this.Directory = this.Directory +@"\";


            // Obtain the runtime value of the Text input argument
            DirectoryInfo di = new DirectoryInfo(this.Directory);
            FileInfo[] files = di.GetFiles(this.SearchPattern);

            Helpers.Logger.TestStepDetail("{0} files were found matching the File Mask: \"{1}\" in the directory: \"{2}\"", files.Length, this.SearchPattern, this.Directory);

            
            foreach (FileInfo file in files)
            {
                System.IO.File.Delete(file.FullName);
                Helpers.Logger.TestStepDetail("File: \"{0}\" was successfully deleted.", file.FullName);
            }
        }

        #endregion

        #region Metadata
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if(string.IsNullOrEmpty(Directory))
                metadata.AddValidationError(string.Format(Properties.Resources.VALIDATION_PROPERTY,"Directory"));

            if(string.IsNullOrEmpty(SearchPattern))
                metadata.AddValidationError(string.Format(Properties.Resources.VALIDATION_PROPERTY, "SearchPattern"));
        }

        static FileDelete()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(FileDelete),
                "Directory",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.DirectoryPickerDialog)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
       
        }
        #endregion

    }
}
