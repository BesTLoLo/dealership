using Microsoft.AspNetCore.Components.Forms;

namespace DealershipManagement.Services
{
    public interface IFileService
    {
        Task<string> UploadInvoiceAsync(IBrowserFile file);
        Task<byte[]?> DownloadInvoiceAsync(string filePath);
        Task<bool> DeleteInvoiceAsync(string filePath);
        bool IsValidFileType(IBrowserFile file);
        long GetMaxFileSize();
    }
}
