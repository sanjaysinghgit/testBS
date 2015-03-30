using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Email
{
    public class EmailSender
    {
        public void Send(MailMessage message)
        {
            try
            {
                NetworkCredential cred = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUsername"], ConfigurationManager.AppSettings["SMTPPassword"]);
                var smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
                var smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                client.UseDefaultCredentials = false;
                client.Credentials = cred; // Send our account login details to the client.
                client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]); ;   // Read below.
                client.Send(message);
                //client.SendAsync()
            }
            catch (System.Exception ex)
            {
                throw;
            }
            
        }
    }
}
