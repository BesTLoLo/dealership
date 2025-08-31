using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.StaticFiles;

namespace DealershipManagement.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath;
        private readonly string[] _allowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadPath = Path.Combine(_environment.WebRootPath, "invoices");
            
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);
        }

        public async Task<string> UploadInvoiceAsync(IBrowserFile file)
        {
            if (file == null)
                throw new ArgumentException("File is empty");

            if (!IsValidFileType(file))
                throw new ArgumentException("Invalid file type");

            if (file.Size > MaxFileSize)
                throw new ArgumentException("File size exceeds maximum allowed size");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.Name)}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream(MaxFileSize).CopyToAsync(stream);
            }

            return $"/invoices/{fileName}";
        }

        public async Task<byte[]?> DownloadInvoiceAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            
            if (!File.Exists(fullPath))
                return null;

            return await File.ReadAllBytesAsync(fullPath);
        }

        public Task<bool> DeleteInvoiceAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.FromResult(false);

            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            
            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            try
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public bool IsValidFileType(IBrowserFile file)
        {
            if (file == null)
                return false;

            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }

        public long GetMaxFileSize()
        {
            return MaxFileSize;
        }
    }
}
