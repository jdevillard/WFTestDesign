using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using System.Windows.Forms;

namespace WFTestDesign.Activities.UI
{
    class DirectoryPickerDialog: DialogPropertyValueEditor
    {
            
            private Resource.EditorResources res = new Resource.EditorResources();
            public DirectoryPickerDialog()
            {
                this.InlineEditorTemplate = res["CustomInlineEditor"] as DataTemplate;
            }

            public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
            {
                FolderBrowserDialog ofd = new FolderBrowserDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    propertyValue.StringValue = ofd.SelectedPath;
                }
            }
        
    }
}
