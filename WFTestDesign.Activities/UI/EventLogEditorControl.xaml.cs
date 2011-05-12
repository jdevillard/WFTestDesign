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
using WFTestDesign.Activities.Helpers;
using System.Collections.ObjectModel;


namespace WFTestDesign.Activities.UI
{
    /// <summary>
    /// Interaction logic for EventLogEditor.xaml
    /// </summary>
    public partial class EventLogEditorControl : Window
    {
        private ObservableCollection<Helpers.EventLogItem> _eventlogCollection;
        public ObservableCollection<Helpers.EventLogItem> eventlogCollection
        {
            get
            {
                if (_eventlogCollection == null)
                {
                    _eventlogCollection = new EventLogItemCollection();
                }
                
                    return _eventlogCollection;
            }
            set
            {
                //_eventlogCollection = new ObservableCollection<EventLogItem>(value);
                _eventlogCollection = value;
            }
        }
        
        public EventLogEditorControl()
        {
            InitializeComponent();

          //  _eventlogCollection = new EventLogItemCollection();
      /*      
            _eventlogCollection.Add(new EventLogItem() 
            {Name ="test1",
                EventId=12 });

            eventlogCollection.Add(new EventLogItem()
            {
                Name = "test2",
                EventId = 122
            }); 
             */
 //           System.Diagnostics.Debug.WriteLine("property count EventLogEditorControl() : " + _eventlogCollection.Count);
        }



        public void setEventLogCollection(ObservableCollection<Helpers.EventLogItem> collection)
        {
            System.Diagnostics.Debug.WriteLine("property count : " + collection.Count);
            _eventlogCollection = collection;

            listBox1.ItemsSource = _eventlogCollection;
            listBox1.DisplayMemberPath = "Name";
        }

        public ObservableCollection<Helpers.EventLogItem> GetEventLogCollection()
        {
            return this.eventlogCollection;
        }
        public EventLogEditorControl Clone()
        {
            return (EventLogEditorControl)this.MemberwiseClone();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Helpers.EventLogItem> internalCollection;
            internalCollection = new ObservableCollection<EventLogItem>(this._eventlogCollection);

            eventlogCollection.Clear();
            eventlogCollection = internalCollection;

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Call AddMethod ");
            _eventlogCollection.Add(new EventLogItem()
            {
                Name = "New Item"
            }
            );
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Call RemoveMethod ");
            _eventlogCollection.Remove((listBox1.SelectedItem as EventLogItem));
        }
    }

    
}
