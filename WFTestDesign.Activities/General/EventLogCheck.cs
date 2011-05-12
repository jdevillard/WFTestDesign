using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace WFTestDesign.Activities.General
{

    public sealed class EventLogCheck : CodeActivity
    {

        private ObservableCollection<Helpers.EventLogItem> _eventLogCollection;
        private Helpers.EventLogWatch _watcher;
        // Define an activity input argument of type string
        public InOutArgument<WFTestDesign.Activities.Helpers.EventLogWatch> watcher { get; set; }


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            _watcher = watcher.Get(context);

            _watcher.checkEventCatched();

        }

        
/*
        private void Init()
        {
            if ((null == this.machine) || (this.machine.Length == 0))
            {
                this.machine = Environment.MachineName;
            }

           
        }
        */
        /*
        private void execute()
        {
            if (this.delayBeforeCheck > 0)
            {
               // context.LogInfo("Waiting for {0} seconds before checking the event log.", this.delayBeforeCheck);
                System.Threading.Thread.Sleep(this.delayBeforeCheck * 1000);
            }

            EventLogEntryType entryType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), this.type, true);

            //DateTime cutOffTime = context.TestCaseStart;
            // Note: event log time is always truncated, so the cut off time also need sto be!
            //cutOffTime = cutOffTime.Subtract(new TimeSpan(0, 0, 0, 0, context.TestCaseStart.Millisecond + 1));

            bool found = false;
            using (EventLog log = new EventLog(this.eventLog, this.machine))
            {
                EventLogEntryCollection entries = log.Entries;

              //  context.LogInfo("Scanning {0} event log entries from log: '{1}' on machine: '{2}', cutOffTime: '{3}'.", entries.Count, this.eventLog, this.machine, cutOffTime.ToString("HH:mm:ss.fff dd/MM/yyyy"));
                for (int i = entries.Count - 1; i >= 0; i--)
                {
                    EventLogEntry entry = entries[i];
                    if (0 > (DateTime.Compare(entry.TimeGenerated, cutOffTime)))
                    {
                //        context.LogInfo("Scanning of event log stopped, event.TimeGenerated: {0}, cutOffTime: {1}", entry.TimeGenerated.ToString("HH:mm:ss.fff dd/MM/yyyy"), cutOffTime.ToString("HH:mm:ss.fff dd/MM/yyyy"));
                        found = false;
                        break;
                    }

                  //  context.LogInfo("Checking entry, Source: {0}, EntryType: {1}, EventId: {2}", entry.Source, entry.EntryType, entry.InstanceId);

                    // Note: EventId is optional...
                    if (((entry.Source == this.source) && (entry.EntryType == entryType)) &&
                         (((this.eventId > 0) && (entry.InstanceId == this.eventId)) || (this.eventId == 0)))
                    {
                        foreach (string validationRegex in this.validationRegexs)
                        {
                            string matchPattern = validationRegex;
                            Match match = Regex.Match(entry.Message, matchPattern);

                            if (match.Success)
                            {
                                found = true;
                             //   context.LogInfo("Successfully matched event log entry generated at '{0}'.", entry.TimeGenerated);
                             //   context.LogData("Event log entry.", entry.Message);
                                break;
                            }
                            else
                            {
                                found = false;
                            }
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }
            }

            // Check that its ok
            if (!this.failIfFound && !found)
            {
                throw new ApplicationException("Failed to find expected event log entry.");
            }
            else if (this.failIfFound && found)
            {
                throw new ApplicationException("Found event log entry which should not be present.");
            }
        }
    
         */ 
          }
}
