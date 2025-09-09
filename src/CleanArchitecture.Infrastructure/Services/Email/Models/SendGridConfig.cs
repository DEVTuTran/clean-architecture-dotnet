namespace CleanArchitecture.Infrastructure.Services.Email.Models;

public class SendGridConfig
{
    public string Sender { get; set; } = string.Empty;
    public string TemplateInviteUser { get; set; } = string.Empty;
    public string TemplateContactUsForwardToAdmin { get; set; } = string.Empty;
    public string TemplateContactUs { get; set; } = string.Empty;
    public string TemplateNotifyDataUpdated { get; set; } = string.Empty;
    public string SenderDisplayName { get; set; } = string.Empty;
    public string AdminMail { get; set; } = string.Empty;
    public string AdminMailDisplayName { get; set; } = string.Empty;
}
