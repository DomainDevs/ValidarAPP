
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Mail
{
    public class MailService
    {
        public bool SendMail(string FilePath, string email, string SmtpServer, int PortNum, string strUserName, string strSenderName, string strPassword,
            string strEmailFrom, string messageBody)
        {

            try
            {
                List<MailAddress> To = new List<MailAddress>();
                MailAddress destiny = new MailAddress(email, "");
                To.Add(destiny);

                System.Net.NetworkCredential Auth = new System.Net.NetworkCredential(strUserName, strPassword);

                MailAddress From = new MailAddress(strEmailFrom, strSenderName);

                // Se Configura el servidor SMTP.
                SmtpClient SC = new SmtpClient(SmtpServer, PortNum);
                SC.EnableSsl = true;
                // Se establece en False la Conexión segura ya que el servidor no la soporta.
                SC.UseDefaultCredentials = true;

                // Se Crea el mensaje de email.
                using (MailMessage message = new MailMessage())
                {

                    message.From = From; // Se añade el remitente.

                    // Se añaden los destinatarios.
                    foreach (MailAddress ma in To)
                    {
                        message.To.Add(ma);
                    }
                    if (messageBody != string.Empty && !string.IsNullOrEmpty(FilePath))
                    {
                        string file = Path.GetFileName(FilePath); //Se obtiene el nombre del archivo que se va a adjuntar.
                        message.Subject = "Enviando " + file; // Se establece el Asunto del email.

                        // Se establece el cuerpo del correo.
                        message.Body = messageBody;
                            
                        message.Attachments.Add(new System.Net.Mail.Attachment(FilePath)); // Se agrega el archivo como archivo adjunto.
                    }
                    else
                    {
                        message.Subject = "Servicio Listas de Riesgo Sistran";
                        message.Body = messageBody;
                    }

                    if (SC.UseDefaultCredentials)
                    {
                        SC.Credentials = Auth; // Se indican las credenciales para autenticarse.
                    }
                    SC.DeliveryMethod = SmtpDeliveryMethod.Network; // Se indica el modo de envío.
                    SC.Timeout = 100000;//Se establece el tiempo de espera.
                    SC.Send(message); // Se envia el email.
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void SendMailEws(string smtpServer, int portNumber, string ToEmail, string UsernameEmail, string UsernamePassword, string subject, string message, string attachmentFilename, string emailCC)
        {
            try
            {

                ExchangeService service = new ExchangeService { Credentials = new NetworkCredential(UsernameEmail.Split('@')[0], UsernamePassword) };
                service.AutodiscoverUrl(UsernameEmail, RedirectionUrlValidationCallback);
                EmailMessage emailMessage = new EmailMessage(service);
                emailMessage.ToRecipients.Add(ToEmail);
                emailMessage.Subject = subject;
                emailMessage.Body = new MessageBody(BodyType.HTML, message);
                if (!string.IsNullOrEmpty(attachmentFilename))
                {
                    emailMessage.Attachments.AddFileAttachment(attachmentFilename);
                }
                emailMessage.SendAndSaveCopy();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }



    }
}
