using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace WFTestDesign.Activities.UI
{
    class XmlValidationEditorDialog : DialogPropertyValueEditor
    {
        private Resource.EditorResources res = new Resource.EditorResources();

        public XmlValidationEditorDialog()
            {
                this.InlineEditorTemplate = res["CustomInlineEditor"] as DataTemplate;
            }
 
        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
            {
                
                XmlValidationEditorControl control = new XmlValidationEditorControl();
                
                if (propertyValue.Value != null)
                    control.setCollection((ObservableCollection<string>)propertyValue.Value);
 
                control.ShowDialog();
                
                if(control.DialogResult.HasValue && control.DialogResult.Value)
                {
                   propertyValue.Value = control.xpathCollection;
                }
            }
    }
}
