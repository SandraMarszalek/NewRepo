using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebApplication8.Resource.Constants;

namespace WebApplication8.Areas.Identity.Email
{
    public class EmailHelper
    {
        public bool SendEmail(string Email, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("mobilnyasystentrodziny@gmail.com");
            mailMessage.To.Add(new MailAddress(Email));

            mailMessage.Subject = "Potwierdź adres e-mail";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "To ostatni krok! Potwierdź swój adres mailowy i zacznij korzystać z aplikacji! " + confirmationLink;

            var credential = new NetworkCredential
            {
                UserName = EmailInfo.FROM_EMAIL_ACCOUNT,
                Password = EmailInfo.FROM_EMAIL_PASSWORD
            };

            SmtpClient client = new SmtpClient();

            client.Credentials = credential;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public bool SendEmailReset(string Email, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("mobilnyasystentrodziny@gmail.com");
            mailMessage.To.Add(new MailAddress(Email));

            mailMessage.Subject = "Zresetuj hasło";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Zresetuj hasło do swojego konta klikając w link " + confirmationLink;

            var credential = new NetworkCredential
            {
                UserName = EmailInfo.FROM_EMAIL_ACCOUNT,
                Password = EmailInfo.FROM_EMAIL_PASSWORD
            };

            SmtpClient client = new SmtpClient();

            client.Credentials = credential;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
    }
}
