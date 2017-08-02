using Revive.Redux.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Common
{
    public class EmailHelper
    {
        public void Send(string toAddress, string ccAddress, string bccAddress, string subject, string body)
        {
            Task.Factory.StartNew(() => SendEmail(toAddress, ccAddress, bccAddress, subject, body, false), TaskCreationOptions.LongRunning);
        }

       

        private void SendEmail(string toAddress, string ccAddress, string bccAddress, string subject, string body, bool priority)
        {
            // bccAddress = "leveltwomanager@svam.com;" + bccAddress;   //added for vishal sir notification.

            // added code temporary for emailing to hardcoded ID
            //toAddress = "Santosh.Kumar@svam.com";
            //ccAddress = "ksahoo@svam.com";
            //bccAddress = "ksahoo@svam.com";

            try
            {
                MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpFromAddress"], ConfigurationManager.AppSettings["SmtpUserDisplayName"]);
                string[] emailTo = toAddress.Split(ConstantEntities.EmailSeparator);
                string[] emailCC = ccAddress.Split(ConstantEntities.EmailSeparator);
                string[] emailBcc = bccAddress.Split(ConstantEntities.EmailSeparator);
                string serverName = ConfigurationManager.AppSettings["SmtpServerName"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                string userName = ConfigurationManager.AppSettings["SmtpUserName"];
                string password = ConfigurationManager.AppSettings["SmtpPassword"];

                MailMessage message = new MailMessage();
                message.From = fromAddress;
                message.Subject = subject;
                message.Body = body;
                for (int i = 0; i < emailTo.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailTo[i]))
                        message.To.Add(emailTo[i]);
                }
                for (int i = 0; i < emailCC.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailCC[i]))
                        message.CC.Add(emailCC[i]);
                }
                for (int i = 0; i < emailBcc.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailBcc[i]))
                        message.Bcc.Add(emailBcc[i]);
                }
                message.IsBodyHtml = true;
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                if (priority) message.Priority = MailPriority.High;


                SmtpClient client = new SmtpClient(serverName, port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpSsl"]);
                client.UseDefaultCredentials = false;

                NetworkCredential smtpUserInfo = new NetworkCredential(userName, password);
                client.Credentials = smtpUserInfo;
                client.Send(message);

                client.Dispose();
                message.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Send checkout results
        public void SendCheckOutResults(string toAddress, string ccAddress, string bccAddress, string subject, string body)
        {
            Task.Factory.StartNew(() => SendEmailResults(toAddress, ccAddress, bccAddress, subject, body, false), TaskCreationOptions.LongRunning);
        }
        private void SendEmailResults(string toAddress, string ccAddress, string bccAddress, string subject, string body, bool priority)
        {
            // bccAddress = "leveltwomanager@svam.com;" + bccAddress;   //added for vishal sir notification.

            // added code temporary for emailing to hardcoded ID
            //toAddress = "Santosh.Kumar@svam.com";
            //ccAddress = "ksahoo@svam.com";
            //bccAddress = "ksahoo@svam.com";

            try
            {
                MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpFromAddressCheckoutResults"], ConfigurationManager.AppSettings["SmtpUserDisplayNameCheckoutResults"]);
                string[] emailTo = toAddress.Split(ConstantEntities.EmailSeparator);
                string[] emailCC = ccAddress.Split(ConstantEntities.EmailSeparator);
                string[] emailBcc = bccAddress.Split(ConstantEntities.EmailSeparator);
                string serverName = ConfigurationManager.AppSettings["SmtpServerName"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                string userName = ConfigurationManager.AppSettings["SmtpUserName"];
                string password = ConfigurationManager.AppSettings["SmtpPassword"];

                MailMessage message = new MailMessage();
                message.From = fromAddress;
                message.Subject = subject;
                message.Body = body;
                for (int i = 0; i < emailTo.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailTo[i]))
                        message.To.Add(emailTo[i]);
                }
                for (int i = 0; i < emailCC.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailCC[i]))
                        message.CC.Add(emailCC[i]);
                }
                for (int i = 0; i < emailBcc.Length; i++)
                {
                    if (!string.IsNullOrEmpty(emailBcc[i]))
                        message.Bcc.Add(emailBcc[i]);
                }
                message.IsBodyHtml = true;
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                if (priority) message.Priority = MailPriority.High;


                SmtpClient client = new SmtpClient(serverName, port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpSsl"]);
                client.UseDefaultCredentials = false;

                NetworkCredential smtpUserInfo = new NetworkCredential(userName, password);
                client.Credentials = smtpUserInfo;
                client.Send(message);

                client.Dispose();
                message.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
