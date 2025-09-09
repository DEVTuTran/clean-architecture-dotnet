using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Office;

public interface IExcelService
{
    Task<string> CloneExcelAsync(string s3KeySource);
    Task CopyRootExcelAsync(string s3Key);
    Task<string> ExportExcelFileAsync(string s3Key, string filename);
    Task<object> GetExcelDataAsync(string s3Key, object excelData);
    IEnumerable<object> GetExcelImportExport();
    Task<object> GetExcelPackageAsync(string s3Key, bool isTemporary = false);
    Task<object> GetImportExportWorkSheetAsync(int result, string s3Key, bool isTemporary = false);
    Task<object> GetResultAsync(int result, string s3Key);
    Task MergeExcelWorksheetsAsync(string s3Key, bool isRoot = false, IEnumerable<object>? excelDatas = null, bool isMigration = false);
    Task UpdateClientInfoAsync(string s3Key, string clientDataName, string clientName);
    Task<object> UpdateExcelDataAsync(string s3Key, object excelData);
    Task UpdateResult9Async(object[] results, string s3Key, int resultIndex);
}


