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

namespace WFTestDesign.Activities.UI
{
    /// <summary>
    /// Interaction logic for XmlValidationEditorControl.xaml
    /// </summary>
    public partial class XmlValidationEditorControl : Window
    {

        private ObservableCollection<string> _xpathCollection;
        public ObservableCollection<string> xpathCollection
        {
            get
            {
                if (_xpathCollection == null)
                {
                    _xpathCollection = new ObservableCollection<string>();
                }

                return _xpathCollection;
            }
            set
            {
                //_eventlogCollection = new ObservableCollection<EventLogItem>(value);
                _xpathCollection = value;
            }
        }
        


        public XmlValidationEditorControl()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxName.Text))
            {
                if (!_xpathCollection.Contains(tbxName.Text))
                    _xpathCollection.Add(tbxName.Text);
                else
                    MessageBox.Show("The list already contains this Xpath");
            }
         
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Call RemoveMethod ");
            _xpathCollection.Remove((listBox1.SelectedItem as string));
        }

        public void setCollection(ObservableCollection<string> collection)
        {
            System.Diagnostics.Debug.WriteLine("property count : " + collection.Count);
            _xpathCollection = collection;

            listBox1.ItemsSource = _xpathCollection;
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> internalCollection;
            internalCollection = new ObservableCollection<string>(this._xpathCollection);

            xpathCollection.Clear();
            xpathCollection = internalCollection;

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
