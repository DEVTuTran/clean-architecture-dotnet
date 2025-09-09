namespace CleanArchitecture.Application.Interfaces.Security;

public interface IProtector
{
    string Protected(string text);
    string UnProtected(string text);
}


