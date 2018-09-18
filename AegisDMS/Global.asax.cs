using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AegisDMS
{
    public class Global : System.Web.HttpApplication
    {
        private System.Timers.Timer aTimer;
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(86400000); //24Hr
            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += OnTimer;
            aTimer.Enabled = true;

        }

        public void OnTimer(Object source, ElapsedEventArgs e)
        {
            string randomName = DateTime.Now.Ticks.ToString();
            string htmlFilePath = Server.MapPath("") + "\\Files\\Temp\\" + randomName + ".htm";

            DataTable dt = BusinessLayer.File.GetPendingContentTransferFiles();
            foreach (DataRow dr in dt.Rows)
            {
                string physicalFileName = dr["PhysicalFileName"].ToString();
                string originalFileName = dr["FileOriginalName"].ToString();
                string fileExtension = dr["FileExtension"].ToString();

                //Build the File Path for the original (input) and the decrypted (output) file
                string encryptedPhysicalFileNameWithPath = Server.MapPath("~/Files/") + physicalFileName + fileExtension;
                string decryptOriginalFileNameWithPath = Server.MapPath("~/Files/Raw/") + originalFileName + fileExtension;

                BusinessLayer.GeneralSecurity.DecryptFile(encryptedPhysicalFileNameWithPath, decryptOriginalFileNameWithPath);
                BusinessLayer.FileContentTransfer.StartProcess(Guid.Parse(dr["FileGuid"].ToString()), decryptOriginalFileNameWithPath, htmlFilePath);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}