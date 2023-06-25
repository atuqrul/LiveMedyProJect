using System.Net;
using System.Net.Mail;

namespace Livemedy.Services.Core.Helpers;

public static class Tools
{
    public static bool SendMail(string mailAddress)
    {
        if (!string.IsNullOrEmpty(mailAddress))
        {
            var key = Encryption.Encrypt(mailAddress);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("casetry07@outlook.com");
            mailMessage.To.Add(mailAddress);
            mailMessage.Subject = "Davet Linki";
            mailMessage.Body = $"http://localhost:5063/account/register?em={key}";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.outlook.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("casetry07@outlook.com", "07case07");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }
}
