using Microsoft.Exchange.WebServices.Data;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Sistran.Core.Application.UtilitiesServicesEEProvider.Helper
{
    public static class HelperEmail
    {
        /// <summary>
        /// realiza el envio de un email
        /// </summary>
        /// <param name="email">detalles del email</param>
        /// <returns></returns>
        public static bool SendEmail(EmailCriteria email)
        {
            try
            {
                var isEws = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EWS"));
                var displayName = ConfigurationManager.AppSettings.Get("DisplayName");
                var emailFrom = ConfigurationManager.AppSettings.Get("EmailFrom");
                var password = ConfigurationManager.AppSettings.Get("EMailPass");
                var smtpServer = ConfigurationManager.AppSettings.Get("SMTPServer");
                var portNumber = Convert.ToInt16(ConfigurationManager.AppSettings.Get("SMTPServerPort"));
                var enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableSsl"));
                //var timeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("TimeoutEmail"));

                if (isEws)
                {
                    ExchangeService service = new ExchangeService { Credentials = new NetworkCredential(emailFrom.Split('@')[0], password) };
                    service.AutodiscoverUrl(emailFrom, RedirectionUrlValidationCallback);
                    EmailMessage emailMessage = new EmailMessage(service);

                    foreach (var emailAddress in email.Addressed)
                    {
                        emailMessage.ToRecipients.Add(emailAddress);
                    }

                    emailMessage.Subject = email.Subject;
                    emailMessage.Body = new MessageBody(BodyType.HTML, email.Message);

                    var fileStreams = new List<FileStream>();
                    if (email.Files != null)
                    {
                        foreach (var filepath in email.Files)
                        {
                            var fs = System.IO.File.OpenRead(filepath);
                            //https://msdn.microsoft.com/en-us/library/office/dn726694(v=exchg.150).aspx
                            var filenamefromPath = filepath.Split(new char[] { '\\' }).Last();
                            emailMessage.Attachments.AddFileAttachment(filenamefromPath, fs);
                            fileStreams.Add(fs);
                        }
                    }

                    emailMessage.SendAndSaveCopy();
                    foreach (var fs in fileStreams)
                    {
                        fs.Dispose();
                    }
                }
                else
                {

                    using (MailMessage mail = new MailMessage())
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        mail.From = new MailAddress(emailFrom, displayName);

                        mail.Subject = email.Subject;
                        mail.Body = email.Message;
                        mail.IsBodyHtml = true;

                        foreach (var emailAddress in email.Addressed)
                        {
                            mail.To.Add(emailAddress);
                        }

                        smtp.Host = smtpServer;
                        smtp.Port = portNumber;
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSsl;
                        //smtp.Timeout = timeout;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        if (email.Files != null)
                        {
                            foreach (var file in email.Files)
                            {
                                System.Net.Mail.Attachment data = new System.Net.Mail.Attachment(file, MediaTypeNames.Application.Octet);
                                ContentDisposition disposition = data.ContentDisposition;
                                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                                mail.Attachments.Add(data);
                            }
                        }

                        smtp.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            //bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);
            return redirectionUri.Scheme == "https";
        }
    }
}
