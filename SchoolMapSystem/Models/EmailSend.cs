using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace SchoolMapSystem.Models
{
    class EmailSend
    {
        public void SendEmail(List<string> directories, string username)
        {
            try
            {
                // Create a new MailMessage object
                var message = new MailMessage();

                // Set the sender and recipient email addresses
                message.From = new MailAddress("emailaddress"); // Admin email address/password     
                message.To.Add(username);

                // Set the email subject and body
                message.Subject = "Timetable PDFs";
                message.Body = "Please find attached the timetable PDFs.";

                // Attach the PDF files to the email
                foreach (string directory in directories)
                {
                    var attachment = new Attachment(directory);
                    message.Attachments.Add(attachment);
                }

                // Create a new SmtpClient object and set the SMTP server details
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential("emailaddress", "accountpassword");  // Admin email address/password
                smtpClient.EnableSsl = true;

                // Send the email
                smtpClient.Send(message);

                MessageBox.Show("Email sent successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}");
            }
        }
    }
}