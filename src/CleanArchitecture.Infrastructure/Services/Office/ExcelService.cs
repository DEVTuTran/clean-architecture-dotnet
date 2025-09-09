using CleanArchitecture.Application.Interfaces.Office;
using CleanArchitecture.Application.Interfaces.Storage;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Office;

public class ExcelService : IExcelService
{
    private readonly IS3 _s3Service;

    public ExcelService(IS3 s3Service)
    {
        _s3Service = s3Service;
    }

    public Task<string> CloneExcelAsync(string s3KeySource) { return Task.FromResult($"cloned_{s3KeySource}"); }
    public Task CopyRootExcelAsync(string s3Key) { return Task.CompletedTask; }
    public Task<string> ExportExcelFileAsync(string s3Key, string filename) { return Task.FromResult($"exported_{filename}"); }
    public Task<object> GetExcelDataAsync(string s3Key, object excelData) { return Task.FromResult<object>(new { }); }
    public IEnumerable<object> GetExcelImportExport() { return new List<object>(); }
    public Task<object> GetExcelPackageAsync(string s3Key, bool isTemporary = false) { return Task.FromResult<object>(new { }); }
    public Task<object> GetImportExportWorkSheetAsync(int result, string s3Key, bool isTemporary = false) { return Task.FromResult<object>(new { }); }
    public Task<object> GetResultAsync(int result, string s3Key) { return Task.FromResult<object>(new { }); }
    public Task MergeExcelWorksheetsAsync(string s3Key, bool isRoot = false, IEnumerable<object>? excelDatas = null, bool isMigration = false) { return Task.CompletedTask; }
    public Task UpdateClientInfoAsync(string s3Key, string clientDataName, string clientName) { return Task.CompletedTask; }
    public Task<object> UpdateExcelDataAsync(string s3Key, object excelData) { return Task.FromResult<object>(new { }); }
    public Task UpdateResult9Async(object[] results, string s3Key, int resultIndex) { return Task.CompletedTask; }
}


