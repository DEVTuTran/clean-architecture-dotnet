using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using CleanArchitecture.Application.Interfaces.Email;
using CleanArchitecture.Infrastructure.Services.Email.Models;

namespace CleanArchitecture.Infrastructure.Services.Email;

public class SendGridMailService : IMailService
{
    private readonly ISendGridClient _client;
    private readonly SendGridConfig _sendGridConfig;
    private readonly EmailAddress _sender;

    public SendGridMailService(ISendGridClient sendGridClient, IOptions<SendGridConfig> sendGridConfig)
    {
        _client = sendGridClient;
        _sendGridConfig = sendGridConfig.Value;
        _sender = new EmailAddress(_sendGridConfig.Sender, _sendGridConfig.SenderDisplayName);
    }

    public async Task<EmailResponse> SendInviteMailAsync(string email, string name, string password, string authUrl)
    {
        try
        {
            var emailAddress = new EmailAddress(email, name);
            var msg = MailHelper.CreateSingleTemplateEmail(_sender, emailAddress, _sendGridConfig.TemplateInviteUser, new
            {
                userName = name,
                mailAddress = email,
                password = password,
                authUrl = authUrl
            });
            var response = await _client.SendEmailAsync(msg);

            return new EmailResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                Message = response.IsSuccessStatusCode ? "Email sent successfully" : "Failed to send email"
            };
        }
        catch (Exception ex)
        {
            return new EmailResponse
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<EmailResponse> SendContactUsMailAsync(object contactUs, object organizationUser)
    {
        try
        {
            // This is a placeholder implementation
            // The actual implementation would need the specific domain models
            var adminEmail = new EmailAddress(_sendGridConfig.AdminMail, _sendGridConfig.AdminMailDisplayName);
            var msg = MailHelper.CreateSingleTemplateEmail(_sender, adminEmail, _sendGridConfig.TemplateContactUsForwardToAdmin, new
            {
                // Placeholder data - would need actual domain models
                userName = "User",
                mailAddress = "user@example.com",
                organizationName = "Organization",
                inquiryType = "General",
                replyStatus = "Pending",
                description = "Contact inquiry"
            });
            await _client.SendEmailAsync(msg);

            // Send to user
            msg = MailHelper.CreateSingleTemplateEmail(_sender, new EmailAddress("user@example.com", "User"), _sendGridConfig.TemplateContactUs, new
            {
                userName = "User",
                inquiryType = "General",
                description = "Contact inquiry"
            });

            var response = await _client.SendEmailAsync(msg);

            return new EmailResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                Message = response.IsSuccessStatusCode ? "Contact us email sent successfully" : "Failed to send contact us email"
            };
        }
        catch (Exception ex)
        {
            return new EmailResponse
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<EmailResponse> SendNotifyClientDataUpdatedAsync(object[] users, object organizationUser, object clientData, Uri clientDataUrl)
    {
        try
        {
            // This is a placeholder implementation
            // The actual implementation would need the specific domain models
            var emailAddresses = new List<EmailAddress> { new EmailAddress("user@example.com", "User") };
            var templateData = new List<object> { new
            {
                userName = "User",
                organizationName = "Organization",
                updateName = "Updater",
                updateDateTime = DateTime.UtcNow,
                clientDataName = "Client Data",
                clientDataUrl = clientDataUrl
            }};

            var msg = MailHelper.CreateMultipleTemplateEmailsToMultipleRecipients(_sender, emailAddresses, _sendGridConfig.TemplateNotifyDataUpdated, templateData);
            var response = await _client.SendEmailAsync(msg);

            return new EmailResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                Message = response.IsSuccessStatusCode ? "Notification email sent successfully" : "Failed to send notification email"
            };
        }
        catch (Exception ex)
        {
            return new EmailResponse
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }

    public async Task<EmailResponse> SendTestMail(string email, string body)
    {
        try
        {
            var msg = MailHelper.CreateSingleEmail(_sender, new EmailAddress { Email = email, Name = "Test" }, body, body, body);
            var response = await _client.SendEmailAsync(msg);

            return new EmailResponse
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                Message = response.IsSuccessStatusCode ? "Test email sent successfully" : "Failed to send test email"
            };
        }
        catch (Exception ex)
        {
            return new EmailResponse
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message
            };
        }
    }
}


