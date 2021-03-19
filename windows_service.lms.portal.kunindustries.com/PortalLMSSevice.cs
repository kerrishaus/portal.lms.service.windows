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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows;


namespace windows_service.lms.portal.kunindustries.com
{
    public partial class PortalLMSSevice : ServiceBase
    {
        Timer serviceTimer = new Timer();

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
                    serviceTimer.Interval = Convert.ToDouble(30 * 1000);
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
            sendStatusToAPI();
        }

        protected void sendStatusToAPI()
        {
            const string URL = "https://api.kunindustries.com/portal/lms/class/attendance/status.php";
            string urlParameters = "?classid=7&userid=14&status=0";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                eventLog1.WriteEntry("Sent status to server.", EventLogEntryType.Information);
            }
            else
            {
                eventLog1.WriteEntry("Failed to sent status to server: (" + response.StatusCode + ") " + response.ReasonPhrase, EventLogEntryType.Error);
            }

            // Make any other calls using HttpClient here.

            // Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
