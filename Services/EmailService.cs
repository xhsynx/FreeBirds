using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using FreeBirds.Models;
using Microsoft.Extensions.Options;

namespace FreeBirds.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public string GeneratePasswordResetEmailBody(string username, string resetToken)
        {
            return $@"
                <html>
                <body>
                    <h2>Merhaba {username},</h2>
                    <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
                    <p><a href='http://localhost:5001/reset-password?token={resetToken}'>Şifremi Sıfırla</a></p>
                    <p>Bu bağlantı 24 saat boyunca geçerlidir.</p>
                    <p>Eğer bu isteği siz yapmadıysanız, bu e-postayı görmezden gelebilirsiniz.</p>
                    <br>
                    <p>Saygılarımızla,</p>
                    <p>FreeBirds Ekibi</p>
                </body>
                </html>";
        }
    }
} 