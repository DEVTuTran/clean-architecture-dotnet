namespace CleanArchitecture.Application.Interfaces.Email;

using System;
using System.Threading.Tasks;

public interface IMailService
{
    Task<EmailResponse> SendContactUsMailAsync(object contactUs, object organizationUser);
    Task<EmailResponse> SendInviteMailAsync(string email, string name, string password, string authUrl);
    Task<EmailResponse> SendNotifyClientDataUpdatedAsync(object[] users, object organizationUser, object clientData, Uri clientDataUrl);
    Task<EmailResponse> SendTestMail(string email, string body);
}

public class EmailResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}


