using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace AvencaLib
{
    public static class AvencaNetwork
    {
        private static bool configured = false;
        private static int cPort;
        private static string cHost;
        private static bool cSsl;
        private static string cUsername;
        private static string cPassword;
        private static string cFrom;

        public static void Configure(int port, string host, bool ssl, string username, string password, string from)
        {
            cPort = port;
            cHost = host;
            cSsl = ssl;
            cUsername = username;
            cPassword = password;
            cFrom = from;
            configured = true;
        }

        public static void Reset()
        {
            configured = false;
        }

        public static bool SendEmail(string recipient, string subject, string body, string attachment)
        {
            string[] attachments = {attachment};

            return SendEmail(recipient, subject, body, attachments);
        }

        public static bool SendEmail(string recipients, string subject, string body, string[] attachments = null)
        {
            bool result = false;
            SmtpClient client = new SmtpClient();

            if (!configured)
            {
                cPort = 587;
                cHost = "smtp.gmail.com";
                cSsl = true;
                cUsername = "restauranteavenca@gmail.com";
                cPassword = "kimsctizlnqcnlbh";
                cFrom = "Restaurante Avenca<restauranteavenca@gmail.com>";
            }

            try
            {
                client.Port = cPort;
                client.Host = cHost;
                client.EnableSsl = cSsl;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(cUsername, cPassword);

                MailMessage mm = new MailMessage(cFrom, recipients, subject, body);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                double totalSize = 0;

                if (attachments != null)
                {
                    for (int i = 0; i < attachments.Length; i++)
                    {
                        Attachment att = new Attachment(attachments[i], (string)null);
                        mm.Attachments.Add(att);
                        totalSize += att.ContentStream.Length;
                    }
                    int timeout = (int)(totalSize / 100 * 2);
                    client.Timeout = (timeout < 30000) ? 30000 : timeout;
                }
                client.Send(mm);

                for (int i = 0; i < mm.Attachments.Count; i++)
                    mm.Attachments[i].Dispose();

                result = true;
            }
            catch (Exception ex)
            {
                AvencaErrorHandler.eventLogError(ex);
            }
            finally
            {
                client.Dispose();
            }
            return result;
        }
    }
}
