using JwtToken.jwtDbContect;
using JwtToken.Models;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JwtToken.Services
{
    public class JwtGenrate : IJwtGenrate
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public JwtGenrate(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _context = applicationDbContext;
            _configuration = configuration;
        }

        public bool NewUser(User user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Add user to Users table
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    // Generate password and send email with password
                    var generatedPassword = GeneratePasswordAndSendEmail(user.Email);
                    var userPassword = new UserPassword
                    {
                        UserID = user.UserID,
                        Password = generatedPassword
                    };
                    _context.UserPassword.Add(userPassword);
                    _context.SaveChanges();

                    // Commit transaction
                    transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    // Rollback transaction in case of exception
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Function to generate a random password and send it via email
        private string GeneratePasswordAndSendEmail(string email)
        {
            // Generate a random password
            string newPassword = GenerateRandomPassword();

            // Send email with the new password
            SendPasswordResetEmail(email, newPassword);

            // Return the generated password for reference
            return newPassword;
        }

        // Function to generate a random password
        private string GenerateRandomPassword()
        {
            // Define characters for password generation
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] password = new char[8]; // Length of the generated password

            // Populate the password array with random characters
            for (int i = 0; i < password.Length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }

        private int GetNextPrimaryKeyValue()
        {
            // Get the maximum UserID from Users table and increment by 1
            var maxUserId = _context.Users.Max(u => u.UserID);
            return maxUserId;
        }

        // Function to send email with the generated password
        public void SendPasswordResetEmail(string email, string password)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("JwtToken", "examplegmail")); // Replace with your email address and name
                message.To.Add(new MailboxAddress("", email)); // Recipient's email address
                message.Subject = "Your New Password";
                message.Body = new TextPart("plain")
                {
                    Text = $"Dear User,\n\nYour new password is: {password}\n\nPlease change it after logging in for security reasons.\n\nBest Regards,\nYour Application Team"
                };

                // Convert MimeMessage to System.Net.Mail.MailMessage
                var mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.From = new System.Net.Mail.MailAddress("examplegmail.com"); // Replace with your email address
                mailMessage.To.Add(new System.Net.Mail.MailAddress(email));
                mailMessage.Subject = message.Subject;
                mailMessage.Body = message.TextBody;

                // Send email using System.Net.Mail.SmtpClient
                using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true; // Gmail requires SSL
                    client.Credentials = new System.Net.NetworkCredential("examplegmail.com", "ywyoyoiqemsvscms"); // Replace with your SMTP credentials
                    client.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log errors, notify admins)
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }

        }

        public bool UpdateUser(User user)
        {
            if (user != null)
            {
                User existingUser = _context.Users.FirstOrDefault(s => s.UserID == user.UserID);
                if (existingUser != null)
                {
                    existingUser.ConfirmPasswordReset = true;
                    _context.Users.Update(existingUser);
                    _context.SaveChanges();

                    // Generate and send the password reset email
                    var generatedPassword = GeneratePasswordAndSendEmail(existingUser.Email);
                    var userPassword = new UserPassword
                    {
                        UserID = existingUser.UserID,
                        Password = generatedPassword
                    };
                    _context.UserPassword.Update(userPassword);
                    _context.SaveChanges();

                    return true;
                }
            }
            return false;
        }
    }
}
