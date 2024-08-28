using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _mailUsername;
        private readonly string _mailPassword;
        private readonly string _mailHost;
        private readonly int _mailPort;
        private readonly string _angularClientUrl;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mailUsername = _configuration["Mail:Username"];
            _mailPassword = _configuration["Mail:Password"];
            _mailHost = _configuration["Mail:Host"];
            _mailPort = Convert.ToInt32(_configuration["Mail:Port"]);
            _angularClientUrl = _configuration["AngularClientUrl"];
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            using var mail = new MailMessage
            {
                From = new MailAddress(_mailUsername, "NG E-Commerce", Encoding.UTF8),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            foreach (var to in tos)
                mail.To.Add(to);

            using var smtp = new SmtpClient
            {
                Credentials = new NetworkCredential(_mailUsername, _mailPassword),
                Port = _mailPort,
                EnableSsl = true,
                Host = _mailHost
            };

            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            string body = BuildPasswordResetEmailBody(userId, resetToken);
            await SendMailAsync(to, "Password Reset Request", body);
        }

        private string BuildPasswordResetEmailBody(string userId, string resetToken)
        {
            var mail = new StringBuilder();
            mail.AppendLine("Hello,<br>If you requested a password reset, you can reset your password using the link below.<br>");
            mail.AppendLine("<strong><a target=\"_blank\" href=\"");
            mail.Append(_angularClientUrl);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append('/');
            mail.Append(resetToken);
            mail.AppendLine("\">Click here to reset your password...</a></strong><br><br>");
            mail.AppendLine("<span style=\"font-size:12px;\">NOTE: If you did not make this request, please disregard this email.</span><br>Best regards,<br><br>NG - Mini|E-Commerce");
            return mail.ToString();
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName)
        {
            string body = BuildCompletedOrderEmailBody(orderCode, orderDate, userName);
            await SendMailAsync(to, $"Your Order #{orderCode} is Completed", body);
        }

        private string BuildCompletedOrderEmailBody(string orderCode, DateTime orderDate, string userName)
        {
            return $"Dear {userName},<br>Your order with code {orderCode} placed on {orderDate:MMMM dd, yyyy} has been completed and handed over to the shipping company.<br>Thank you for shopping with us!";
        }
    }
}
