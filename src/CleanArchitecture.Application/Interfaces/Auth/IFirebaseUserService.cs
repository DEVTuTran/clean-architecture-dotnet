using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Auth;

public interface IFirebaseUserService
{
    Task<object> BanUserAsync(string uid, bool disabled);
    Task BanUserBulkAsync(object[] users, bool disabled);
    Task DeleteUserAsync(string? uid = null);
    Task<string> GenerateEmailVerificationLinkAsync(string email);
    string GetEmail();
    string GetUID();
    Task<object> GetUserRecordAsync(string? uid = null);
    bool IsEmailVerified();
    Task<object> RegisterAsync(object userRecordArgs);
    Task SetRootRoleAsync(string uid);
    Task SyncRoleAsync(string? uid = null);
}
