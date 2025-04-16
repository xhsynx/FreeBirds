using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using FreeBirds.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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
            // Create email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            // Set email body
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            // Send email using SMTP
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public string GeneratePasswordResetEmailBody(string username, string resetToken)
        {
            // Generate HTML email body for password reset
            return $@"
                <html>
                <body>
                    <h2>Hello {username},</h2>
                    <p>Please click the link below to reset your password:</p>
                    <p><a href='http://localhost:5001/reset-password?token={resetToken}'>Reset Password</a></p>
                    <p>This link will expire in 24 hours.</p>
                    <p>If you did not request this password reset, please ignore this email.</p>
                    <br>
                    <p>Best regards,</p>
                    <p>FreeBirds Team</p>
                </body>
                </html>";
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetToken)
        {
            // In a real application, you would implement email sending logic here
            // For example, using SendGrid, MailKit, or another email service
            await Task.CompletedTask;
        }
    }
} 