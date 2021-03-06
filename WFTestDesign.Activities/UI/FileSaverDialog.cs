﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;

namespace WFTestDesign.Activities.UI
{
    class FileSaverDialog: DialogPropertyValueEditor
    {
            
            private Resource.EditorResources res = new Resource.EditorResources();
            public FileSaverDialog()
            {
                this.InlineEditorTemplate = res["CustomInlineEditor"] as DataTemplate;
            }

            public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
            {
                 Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.Multiselect = false;
                ofd.CheckFileExists = false;
                if (ofd.ShowDialog() == true)
                {
                    propertyValue.StringValue = ofd.FileName;
                }

            }
        
    }
}
