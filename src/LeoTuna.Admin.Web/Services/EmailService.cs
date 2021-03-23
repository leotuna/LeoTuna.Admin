using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace LeoTuna.Admin.Web.Services
{
    public class EmailService
    {
        private MailAddress From { get; } = new MailAddress("example@gmail.com", "LeoTuna.Admin");
        private IConfiguration Configuration { get; }

        public EmailService(
            IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SendPasswordRecovery(string id, string paraEmail)
        {
            id = HttpUtility.UrlEncode(id);

            var fullUrl = MakeRecuperarSenhaUrl(paraEmail, id);

            var to = new MailAddress(paraEmail);

            MailMessage message = new MailMessage(From, to)
            {
                Subject = "Recover password",
                IsBodyHtml = true,
                Body = $"<p>To recover your password, <a href='{fullUrl}'>click here</a>.</p>"
            };

            var client = MakeClient();

            client.Send(message);
        }

        private SmtpClient MakeClient()
        {
            var smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = MakeEmailCredentials();

            return smtp;
        }

        private NetworkCredential MakeEmailCredentials()
        {
            var email = Configuration.GetSection("EmailCredentials:Email");
            var password = Configuration.GetSection("EmailCredentials:Password");

            if (email is null || password is null)
            {
                throw new Exception();
            }

            return new NetworkCredential(email.Value, password.Value);
        }

        private string MakeRecuperarSenhaUrl(string to, string code)
        {
            var baseUrl = Configuration.GetSection("Urls:Base");

            if (baseUrl is null)
            {
                throw new Exception();
            }

            throw new NotImplementedException();
        }
    }
}
