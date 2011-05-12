using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Activities;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Activities.Presentation.Model;
using System.Activities.Presentation;

namespace WFTestDesign.Activities.Designers
{
    // Interaction logic for ValidateFileStep.xaml
    public partial class ValidateFileStep
    {
        private int count;
        public string test;
        private static List<string> newVariables = new List<string>();
        private static List<System.Collections.DictionaryEntry> XpathReq;

        public ValidateFileStep()
        {
            InitializeComponent();
            count = 1;

            this.Loaded += new RoutedEventHandler(ValidateFileStep_Loaded);

           
        }

        void ValidateFileStep_Loaded(object sender, RoutedEventArgs e)
        {
            this.ModelItem.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Value_PropertyChanged);

            if (this.ModelItem.Properties["Text"].ComputedValue != null)
            {
                List<XpathRequest> ltt = new List<XpathRequest>();
                ltt = (List<XpathRequest>)Deserialize(this.ModelItem.Properties["Text"].ComputedValue.ToString(), typeof(List<XpathRequest>));
                bool first = true;
                int i = 0;

                foreach(XpathRequest item in ltt)
                {
                    if (first)
                    {
                        XpathTbFirst.Text = item.Xpath;
                        ValueTbFirst.Text = item.Value;
                        first = false;
                    }
                    else
                    {
                        AddControl(i++,item.Xpath,item.Value);

                    }
                }


            }

            if (this.ModelItem.Properties["XpathReq"] == null)
            {
                List<System.Collections.DictionaryEntry> ListDictionary_ = new List<System.Collections.DictionaryEntry>();
                //System.Collections.DictionaryEntry Dictionary_ = new System.Collections.DictionaryEntry();
                System.Collections.Generic.Dictionary<InArgument<string>, OutArgument<string>> ttttt = new Dictionary<InArgument<string>, OutArgument<string>>();
                ttttt.Add(new InArgument<string>(), new OutArgument<string>());
                /*
                                InArgument<string> Inarg = new InArgument<string>();
                                Inarg = "test";
                                Dictionary_.Key = Inarg;
                                Dictionary_.Value = new OutArgument<string>();
                                ListDictionary_.Add(Dictionary_);
                  */
                string str = "testxpxpa";
                EditingContext ec = new EditingContext();
                ModelTreeManager mtm = new ModelTreeManager(ec); mtm.Load(str);
                ModelItem tt = mtm.Root;
                MessageBox.Show(str);
                this.ModelItem.Properties["XpathReq"].ComputedValue = tt;



            }  

           


        }

        void Value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
            if (this.ModelItem.Properties["Text"].ComputedValue != null)
            {
                List<XpathRequest> ltt = new List<XpathRequest>();
                ltt = (List<XpathRequest>)Deserialize(this.ModelItem.Properties["Text"].ComputedValue.ToString(), typeof(List<XpathRequest>));
                XpathRequest tt = ltt[0];
                XpathTbFirst.Text = tt.Xpath;
                ValueTbFirst.Text = tt.Value;
            }

 
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            count += 1;
            AddControl(count);
            
            

        }

        private void AddControl(int count_)
        {
            AddControl(count_, string.Empty, string.Empty);
        }

        private void AddControl(int count_, string Xpath, string Value)
        {

            StackPanel stkpanel = new StackPanel() { Name = "StackPanel" + count_, HorizontalAlignment = HorizontalAlignment.Center, Orientation = Orientation.Horizontal };
            
            TextBox Xpathtb = new TextBox() { Name = "Xpathtb" + count_, Text = Xpath, Width = 80 };
            stkpanel.Children.Add(Xpathtb);
            Xpathtb.TextChanged += new TextChangedEventHandler(xpath_TextChanged);
            
            stkpanel.Children.Add(new TextBlock() { Text = "=" });

            TextBox ValueTb = new TextBox() { Name = "Valuetb" + count_, Text = Value, Width = 80 };
            stkpanel.Children.Add(ValueTb);
            ValueTb.TextChanged += new TextChangedEventHandler(xpath_TextChanged);
            
            Button btnRemove = new Button() { Name = "btnRemove" + count_, Content = "-" };
            stkpanel.Children.Add(btnRemove);
            btnRemove.Click += new RoutedEventHandler(ButtonRemove_Click);

            //Register the new StackPAnel
            stackPanelRoot.RegisterName("StackPanel" + count_, stkpanel);
            stackPanelRoot.Children.Add(stkpanel);

            SetTextPropertie();
            

        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            Button btnClicked = (Button)sender;
            object parent = btnClicked.Parent;

            if (parent is StackPanel)
            {
                StackPanel stkpanel = parent as StackPanel;
                stackPanelRoot.Children.Remove(stkpanel);
                
            }
            SetTextPropertie();
                       
        }

        private void xpath_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*TextBox tb = (TextBox)sender;
            //this.ModelItem.Properties["Text"].ComputedValue = tb.Text ;
            XpathRequest tt = new XpathRequest();
            tt.Value = value.Text;
            tt.Xpath = xpath.Text;
            
            ltt.Add(tt);
            */

            SetTextPropertie();

        }

        private void SetTextPropertie()
        {
            List<XpathRequest> ltt = new List<XpathRequest>();

            foreach (StackPanel stk in stackPanelRoot.Children)
            {
                XpathRequest item = new XpathRequest();

                foreach (UIElement element in stk.Children)
                {
                    if (element is TextBox)
                    {
                        TextBox tbelement = element as TextBox;
                        if (tbelement.Name.Contains("Xpath"))
                            item.Xpath = tbelement.Text.ToString();
                        else if (tbelement.Name.Contains("Value"))
                            item.Value = tbelement.Text.ToString();
                    }
                }

                ltt.Add(item);
            }

            this.ModelItem.Properties["Text"].ComputedValue = Serialize(ltt);
        }


        /*public struct XpathRequest
        {
            public string Xpath;
            public string Value;
        }*/
        [Serializable]
        public  class XpathRequest
        {
            

            public   string Xpath { get; set; }
            public  string Value { get; set; }
        }

        #region Serialization Helpers

        protected virtual object Deserialize(string value, Type type)
        {
            using (XmlTextReader textReader = new XmlTextReader(new StringReader(value)))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreProcessingInstructions = true;
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    return serializer.Deserialize(xmlReader);
                }
            }
        }

        protected virtual string Serialize(object obj)
        {
            StringBuilder builder = new StringBuilder();

            using (StringWriter textWriter = new StringWriter(builder))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(xmlWriter, obj);
                }
            }

            return builder.ToString();
        }

        #endregion
        
    }
}
