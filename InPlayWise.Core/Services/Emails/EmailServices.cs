using InPlayWiseCommon.Wrappers;
using InPlayWiseCore.IServices;
using InPlayWiseData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace InPlayWiseCore.Services.Emails
{
    public class EmailServices : IEmailServices
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;
        private readonly ILogger<EmailServices> _logger;

        public EmailServices(IConfiguration configuration, AppDbContext db, ILogger<EmailServices> logger, HttpClient httpClient)
        {
            _config = configuration;
            _db = db;
            _logger = logger;
            _httpClient = httpClient;
        }
        public async Task<Result<bool>> SendResetPasswordOtp(string email, string otp)
        {
            string header = "Reset password for InPlayWise";
            string link = $"https://inplaywise.azurewebsites.net/Auth/ResetPassword?email={email}&code={otp}";
            string userName = await GetUserNameByEmail(email);
            try
            {
                //string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //string solutionRoot = Directory.GetParent(rootPath).Parent.Parent.Parent.FullName;
                //string htmlTemplatePath = Path.Combine(solutionRoot, "InPlayWise.Common", "EmailTemplate", "password-reset.html");
                //string htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath);

                string htmlTemplate = "";

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "password-reset.html");

                // Check if the file exists
                if (File.Exists(templatePath))
                {
                    htmlTemplate =  File.ReadAllText(templatePath);
                }

                htmlTemplate = htmlTemplate.Replace("{link}", link);
                htmlTemplate = htmlTemplate.Replace("*#FirstName#*", userName);

                string logoUrl = "https://inplaywise.azurewebsites.net/WhiteLogo.png";
                var inlineAttachment = await CreateInlineAttachmentFromUrl(logoUrl, "inplaywise-logo");

                bool mailSent = await SendEmailWithInlineImages(email, header, htmlTemplate, new List<Attachment> { inlineAttachment });
                return new Result<bool>
                {
                    IsSuccess = mailSent,
                    StatusCode = mailSent ? 200 : 500,
                    Message = mailSent ? "Successfully sent the link for password reset" : "Some problem occurred during sending mail"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>
                {
                    IsSuccess = false,
                    Message = "Some server side error occurred",
                    StatusCode = 500
                };
            }
        }

        public async Task<bool> SendMagicLink(string userMail)
        {
            try
            {
                string body = string.Empty;
                MailAddress to = new MailAddress(userMail);
                MailAddress from = new MailAddress(_config.GetSection("email:emailAddress").Value);
                MailMessage mailMessage = new MailMessage(from, to);
                mailMessage.Subject = "Login With Direct click";
                string str = "Login With Direct click";
                mailMessage.Body = $"https://localhost:7160/MagicLink?email={userMail}";
                mailMessage.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _config.GetSection("email:smtphost").Value;
                smtp.Port = int.Parse(_config.GetSection("email:smtpport").Value);
                smtp.Credentials = new NetworkCredential(_config.GetSection("email:emailAddress").Value, _config.GetSection("email:password").Value);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }

        }

        public async Task<Result<bool>> SendEmailConfirmationMail(string email, string token)
        {
            string header = "Verify your email for inplaywise";
            string link = $"https://inplaywise.azurewebsites.net/auth/verifyEmail?email={email}&token={token}";

            try
            {

                //string rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //string solutionRoot = Directory.GetParent(rootPath).Parent.Parent.Parent.FullName;
                //string htmlTemplatePath = Path.Combine(solutionRoot, "InPlayWise.Common", "EmailTemplate", "sign-up.html");
                //string htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath);
                string imageUrl = "https://inplaywise.azurewebsites.net/WhiteLogo.png";
                var response = await _httpClient.GetAsync(imageUrl);
                string base64String = "";
                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    base64String = Convert.ToBase64String(imageBytes);
                }

                string htmlTemplate = "";

                var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "sign-up.html");

                // Check if the file exists
                if (File.Exists(templatePath))
                {
                    htmlTemplate = File.ReadAllText(templatePath);
                }

                // Replace placeholders in the template with actual values
                htmlTemplate = htmlTemplate.Replace("{link}", link);
                //htmlTemplate = htmlTemplate.Replace("{BASE64_ENCODED_IMAGE}", base64String);

                string logoUrl = "https://inplaywise.azurewebsites.net/WhiteLogo.png";
                var inlineAttachment = await CreateInlineAttachmentFromUrl(logoUrl, "inplaywise-logo");

                bool mailSent = await SendEmailWithInlineImages(email, header, htmlTemplate, new List<Attachment> { inlineAttachment });


                // Send email with HTML template as body
                
                return new Result<bool>
                {
                    IsSuccess = mailSent,
                    StatusCode = mailSent ? 200 : 500,
                    Message = mailSent ? "Successfully sent the Email Verification Link" : "Some problem occurred during sending mail"
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
                return new Result<bool>
                {
                    IsSuccess = false,
                    Message = "Some server side error occurred",
                    StatusCode = 500
                   
                };
            }
        }

        //UpdatedMethod
        public async Task<bool> SendEmail(string receiverEmail, string header, string body)
        {
            try
            {
                MailAddress to = new MailAddress(receiverEmail);
                MailAddress from = new MailAddress(_config.GetSection("email:emailAddress").Value);
                MailMessage mailMessage = new MailMessage(from, to);
                mailMessage.Subject = header;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _config.GetSection("email:smtphost").Value;
                smtp.Port = int.Parse(_config.GetSection("email:smtpport").Value);
                smtp.Credentials = new NetworkCredential(_config.GetSection("email:emailAddress").Value, _config.GetSection("email:password").Value);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetUserNameByEmail(string email)
        {
            try
            {
                // Assuming there's a User entity with a property for email and username
                var user = await _db.Users
                    .Where(u => u.Email == email)
                    .Select(u => u.UserName)
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine(ex);
                throw; // Rethrow the exception or handle it as needed
            }
        }

        public async Task<Result<bool>> SendNewEmailConfirmationMail(string email, string token)
        {

            string header = "Verify your email for inplaywise";
            string link = $"https://inplaywise.azurewebsites.net/auth/VerifyNewEmail?email={email}&token={token}";

            string imageUrl = "https://inplaywise.azurewebsites.net/WhiteLogo.png";
            var response = await _httpClient.GetAsync(imageUrl);
            string base64String = "";
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                base64String = Convert.ToBase64String(imageBytes);
            }

            string htmlTemplate = "";
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "sign-up.html");

            // Check if the file exists
            if (File.Exists(templatePath))
            {
                htmlTemplate = File.ReadAllText(templatePath);
            }

            // Replace placeholders in the template with actual values
            htmlTemplate = htmlTemplate.Replace("{link}", link);
            htmlTemplate = htmlTemplate.Replace("{BASE64_ENCODED_IMAGE}", base64String);




            //string header = "Verify your email for inplaywise";
            //string link = $"https://inplaywise.azurewebsites.net/auth/VerifyNewEmail?email={email}&token={token}";
            // string body = "The following is the link for verification of Updating email for InPlayWise app. Only click on it if its you who have requested to change email.\n " + link;

            try
            {
                bool mailSent = await SendEmail(email, header, htmlTemplate);
                return new Result<bool>
                {
                    IsSuccess = mailSent,
                    StatusCode = mailSent ? 200 : 500,
                    Message = mailSent ? "Successfully sent the Email Verification Link" : "Some problem occurred during sending mail"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Result<bool>
                {
                    IsSuccess = false,
                    Message = "Some server side error occurred",
                    StatusCode = 500
                };
            }
        }

        private async Task<Attachment> CreateInlineAttachmentFromUrl(string imageUrl, string contentId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Download the image as a byte array
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    // Convert the byte array into a memory stream
                    var memoryStream = new MemoryStream(imageBytes);

                    // Create the attachment
                    var inlineAttachment = new Attachment(memoryStream, "inline-image.png", "image/png")
                    {
                        ContentId = contentId,
                        ContentDisposition =
                {
                    Inline = true,
                    DispositionType = DispositionTypeNames.Inline
                }
                    };

                    return inlineAttachment;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading or attaching image from URL: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> SendEmailWithInlineImages(
    string receiverEmail,
    string header,
    string body,
    List<Attachment> inlineAttachments)
        {
            try
            {
                // Create mail addresses
                MailAddress to = new MailAddress(receiverEmail);
                MailAddress from = new MailAddress(_config.GetSection("email:emailAddress").Value);

                // Create the mail message
                MailMessage mailMessage = new MailMessage(from, to)
                {
                    Subject = header,
                    Body = body,
                    IsBodyHtml = true
                };

                // Add inline attachments
                foreach (var attachment in inlineAttachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }

                // SMTP client configuration
                SmtpClient smtp = new SmtpClient
                {
                    Host = _config.GetSection("email:smtphost").Value,
                    Port = int.Parse(_config.GetSection("email:smtpport").Value),
                    Credentials = new NetworkCredential(
                        _config.GetSection("email:emailAddress").Value,
                        _config.GetSection("email:password").Value),
                    EnableSsl = true
                };

                // Send the email
                await smtp.SendMailAsync(mailMessage);

                // Dispose attachments after sending the email
                foreach (var attachment in inlineAttachments)
                {
                    attachment.ContentStream.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }



    }
}
