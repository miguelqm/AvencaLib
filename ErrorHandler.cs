using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AvencaLib
{
    public static class AvencaErrorHandler
    {
        public static bool ShowErrorMsg;

        public static void eventLogError(Exception ex, string productName = "", bool showMsg = true)
        {
            ShowErrorMsg = showMsg;

            try
            {
                string sSource = productName == "" ? "AVENCA." + Application.ProductName : productName;
                string sLog = "Application";
                string sEvent = ex.Message + ex.StackTrace;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error, 666);

                if (ShowErrorMsg)
                    MessageBox.Show(ex.Message + ex.StackTrace);
            }
            catch (Exception e)
            {
                if (ShowErrorMsg)
                    MessageBox.Show(e.Message + ex.StackTrace);
            }
        }
    }
}
