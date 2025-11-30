using System;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;

namespace BeautySalonApp.Services
{
    public static class EmailService
    {
        public static bool SendReport(string toEmail, string attachmentPath)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Салон Красоты Престиж", "romaalla6@gmail.com"));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = $"Отчет салона красоты - {DateTime.Now:dd.MM.yyyy}";

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = $"Отчет салона красоты 'Престиж' за {DateTime.Now:dd.MM.yyyy}\n\nФайл отчета прикреплен.";

                if (!string.IsNullOrEmpty(attachmentPath) && File.Exists(attachmentPath))
                {
                    bodyBuilder.Attachments.Add(attachmentPath);
                }

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("romaalla6@gmail.com", "vywn qdzv zhzz qicx"); // ЗАМЕНИТЕ
                    client.Send(message);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки email: {ex.Message}\n\nНастройте Gmail:\n" +
                    "1. Включите 2FA\n2. Создайте пароль приложения\n3. Обновите данные в коде",
                    "Ошибка отправки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}