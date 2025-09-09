using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Office;

public interface IPowerPointService
{
    Task<string> ExcelToPowerPointAsync(object clientClientData, object powerPointTheme, bool exportPPtx = false);
    Task<string> GetPreviewPDFAsync(object powerPointTheme, bool exportPPtx = false);
    IPresentation GetSCPresentation();
}

public interface IPresentation
{
    // Placeholder interface for PowerPoint presentation
    // This will be implemented by the actual PowerPoint library
}


