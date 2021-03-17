using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace windows_service.lms.portal.kunindustries.com
{
    public partial class PortalLMSSevice : ServiceBase
    {
        Timer serviceTimer = new Timer();

        String LogSource = "Portal_LMS_Source";

        public PortalLMSSevice()
        {
            InitializeComponent();

            eventLog1 = new System.Diagnostics.EventLog();

            // Turn off autologging
            this.AutoLog = false;

            // create an event source, specifying the name of a log that
            // does not currently exist to create a new, custom log
            if (!System.Diagnostics.EventLog.SourceExists("Portal_LMS_Service_Source"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Portal_LMS_Service_Source", "ServiceLog");
            }

            // configure the event log instance to use this source name
            eventLog1.Source = "Portal_LMS_Service_Source";
            eventLog1.Log = "ServiceLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Starting Portal LMS Service.");
            InitialiseTimer();
            eventLog1.WriteEntry("Started Portal LMS Service.");
        }

        protected override void OnStop()
        {
            serviceTimer.Dispose();
        }

        private void InitialiseTimer()
        {
            try
            {
                if (serviceTimer != null)
                {
                    serviceTimer.AutoReset = true;
                    serviceTimer.Interval = Convert.ToDouble(60 * 1000);
                    serviceTimer.Enabled = true;
                    serviceTimer.Elapsed += serviceTimer_Elapsed;
                    eventLog1.WriteEntry("Portal LMS Service Timer Initialized", EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        protected void serviceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Your Functionality
            eventLog1.WriteEntry("Elapsed Event Called", EventLogEntryType.Information);
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
