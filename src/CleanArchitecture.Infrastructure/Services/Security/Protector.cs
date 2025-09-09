using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using CleanArchitecture.Application.Interfaces.Security;

namespace CleanArchitecture.Infrastructure.Services.Security;

public class Protector : IProtector
{
    private readonly IDataProtector _protector;

    public Protector(IConfiguration configuration, IDataProtectionProvider protectionProvider)
    {
        _protector = protectionProvider.CreateProtector("CleanArchitecture.DataProtection");
    }

    public string Protected(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return _protector.Protect(text);
    }

    public string UnProtected(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        try
        {
            return _protector.Unprotect(text);
        }
        catch
        {
            // Return original text if decryption fails
            return text;
        }
    }
}


