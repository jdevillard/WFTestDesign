using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;

using System.Collections;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Presentation.Metadata;




using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;


namespace WFTestDesign.Activities.General
{
    
    public sealed class EventLogStart : CodeActivity
    {
        // Define an activity input argument of type string
        public InOutArgument<WFTestDesign.Activities.Helpers.EventLogWatch> watcher { get; set; }

        public string EventLogName { get; set; }

        public ObservableCollection<Helpers.EventLogItem> EventToCatch
        {
            get
            {
                if (_eventToCatch == null)
                    _eventToCatch = new ObservableCollection<Helpers.EventLogItem>();

                return _eventToCatch;
            }
            set
            {
                _eventToCatch = value ;
            }
        }

        private ObservableCollection<Helpers.EventLogItem> _eventToCatch;
        
        public string Test { get; set; }


        public EventLogStart()
        {
          
        }

        static EventLogStart()
        {
            AttributeTableBuilder builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(
                typeof(EventLogStart),
                "EventToCatch",
                PropertyValueEditor.CreateEditorAttribute(typeof(UI.EventLogEditorDialog)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
       
        }


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
          

            WFTestDesign.Activities.Helpers.EventLogWatch eventlogwatcher = watcher.Get(context);
            if (eventlogwatcher == null)
                eventlogwatcher = new Helpers.EventLogWatch();
            
            
            eventlogwatcher.EventLogCollection = EventToCatch;
            eventlogwatcher.EventLogName = EventLogName;
            eventlogwatcher.initialisation();
           
            watcher.Set(context, eventlogwatcher);
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            
        }

   
    }
}
