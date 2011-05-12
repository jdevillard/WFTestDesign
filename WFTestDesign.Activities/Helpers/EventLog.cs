using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Eventing.Reader;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;

namespace WFTestDesign.Activities.Helpers
{

    public class EventLogItem
    {
        public string Name
        { get; set; }
        public int EventId { get; set; }
        public string property2 { get; set; }

    }

    public class EventLogItemCollection : ObservableCollection<EventLogItem>
    {
        
    }

    public  class EventLogWatch
    {

        private EventLogWatcher _eventLogWatcher;
        private List<EventRecord> _eventsCatched;
        private string _eventLogName;

        public ObservableCollection<EventLogItem> EventLogCollection {get;set;}

     

        public string EventLogName
        {
            get { return _eventLogName; }
            set { _eventLogName = value; }
        }
        

        public EventLogWatch()
        {
            
        }

       public void initialisation()
       {
           _eventLogWatcher = null;
           _eventsCatched = new List<EventRecord>();
          
           startWatcher();
       }

 
        void startWatcher()
        {
            EventLogQuery subscriptionQuery = new EventLogQuery(_eventLogName, PathType.LogName, "*[System["+createEventIdQuery()+"]]");
            _eventLogWatcher = new EventLogWatcher(subscriptionQuery);
            _eventLogWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(_eventLogWatcher_EventRecordWritten);
            _eventLogWatcher.Enabled = true;
        }

        void _eventLogWatcher_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
           _eventsCatched.Add(e.EventRecord);
            
           System.Diagnostics.Debug.WriteLine(e.EventRecord.ToXml());

           XmlDocument xmldoc = new XmlDocument();
           xmldoc.LoadXml(e.EventRecord.ToXml());

           Helpers.Logger.TestStepDetail("Event Log Catched Id : {0} , message = {1} @ {2}", e.EventRecord.Id, xmldoc.GetElementsByTagName("Data").Item(0).InnerText,System.DateTime.UtcNow.ToString("HH:mm:ss.fff dd/MM/yyyy"));
        }

        public void checkEventCatched()
        {
            stopWatcher();
            
            bool alleventcatched= true;

            foreach (EventLogItem item in EventLogCollection)
            {
                EventRecord result = _eventsCatched.Find(
                    evt => 
                        evt.Id == item.EventId
                    );
                //    delegate(EventRecord evt)
                //    {
                //        return evt.Id == item.EventId;
                //    }
                //);



                if (result != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(result.ToXml());

                    Helpers.Logger.TestStepDetail("Event Log Catched Id : {0} , message = {1}", result.Id, xmldoc.GetElementsByTagName("Data").Item(0).InnerText);
                }
                else
                    alleventcatched = false;

            }
            
            

            if (!alleventcatched)
                throw new Exception("Events not catched");
        }

        void stopWatcher()
        {
            
            _eventLogWatcher.Enabled = false;
            _eventLogWatcher.EventRecordWritten += null;
        }

        string createEventIdQuery()
        {
            int countEventLog = EventLogCollection.Count;
            string eventIdQuery = string.Empty;


            if(countEventLog ==0)
                return string.Empty;
            else if (countEventLog == 1)
                eventIdQuery = "EventID=" + EventLogCollection[0].EventId.ToString();
            else
            {
                eventIdQuery = "EventID=" + EventLogCollection[0].EventId.ToString();
                foreach (EventLogItem item in EventLogCollection)
                {

                    eventIdQuery += " or EventID=" + item.EventId.ToString();

                }
            }
            return eventIdQuery;
              
        }
    }
}
