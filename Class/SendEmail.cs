using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;

namespace e_Travel.Class
{
    public class SendEmail
    {
        public static int SendMail(string ReceiverAddress, string Recsubject, string Recbody,Attachment file)
        {
            try
            {
                System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);

                //Create the SMTP Client
                SmtpClient client = new SmtpClient();
                client.Host = settings.Smtp.Network.Host;
                client.Credentials = credential;
                client.Timeout = 300000;
                client.EnableSsl = true;

                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(settings.Smtp.Network.UserName, "Support Team (eTravel)");
                mm.To.Add(ReceiverAddress);
                mm.Priority = MailPriority.High;
                mm.Attachments.Add(file);

                // Assign the MailMessage's properties
                mm.Subject = Recsubject;
                mm.Body = Recbody;
                mm.IsBodyHtml = true;


                client.Send(mm);
                return 1;
            }

            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}