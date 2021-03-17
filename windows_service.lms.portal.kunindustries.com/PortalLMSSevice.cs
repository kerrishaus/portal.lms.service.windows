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

            if (!System.Diagnostics.EventLog.SourceExists(LogSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(LogSource, "");
            }
            eventLog1.WriteEntry("Portal LMS Service created.");
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
