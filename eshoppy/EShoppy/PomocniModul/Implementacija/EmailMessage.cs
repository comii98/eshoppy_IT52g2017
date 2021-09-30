using EShoppy.PomocniModul.Interfejsi;
using System;
using System.Net.Mail;

namespace EShoppy.PomocniModul.Implementacija
{
    public class EmailMessage : IEmailMessage
    {
        public void SendEmail(string To, string Subject, string Body, bool IsBodyHtml)
        {
            if (string.IsNullOrEmpty(To)) { throw new ArgumentNullException(); }
            if (string.IsNullOrEmpty(Body)) { throw new ArgumentNullException(); }

            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("milicasantrac123@gmail.com"),
                    Subject = Subject,
                    Body = Body,
                    IsBodyHtml = IsBodyHtml
                };

                var smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception($"Nije moguce poslati email obavestenje korisniku. Detalji: {ex.Message}");
            }
        }
    }
}
