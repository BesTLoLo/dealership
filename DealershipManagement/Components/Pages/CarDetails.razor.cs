using BlazorBootstrap;
using DealershipManagement.Models;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DealershipManagement.Components.Pages
{
    public partial class CarDetails
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [Inject]
        private ICarService CarService { get; set; } = default!;

        [Inject]
        private IMaintenanceService MaintenanceService { get; set; } = default!;

        [Inject]
        private IFileService FileService { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ToastService ToastService { get; set; } = default!;

        private Car? car;
        private bool showAddMaintenanceModal = false;
        private MaintenanceLog newMaintenanceLog = new();
        private IBrowserFile? selectedInvoiceFile;

        protected override async Task OnInitializedAsync()
        {
            await LoadCar();
        }

        private async Task LoadCar()
        {
            try
            {
                car = await CarService.GetCarByIdAsync(Id);
                if (car == null)
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Car Not Found",
                        Message = "The requested car could not be found."
                    });
                    Navigation.NavigateTo("/cars");
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to load car details: {ex.Message}"
                });
                Navigation.NavigateTo("/cars");
            }
        }

        private void ShowAddMaintenanceModal()
        {
            newMaintenanceLog = new MaintenanceLog { Date = DateTime.Today };
            showAddMaintenanceModal = true;
        }

        private void CloseAddMaintenanceModal()
        {
            showAddMaintenanceModal = false;
            selectedInvoiceFile = null;
        }

        private void HandleInvoiceFileChange(InputFileChangeEventArgs e)
        {
            selectedInvoiceFile = e.File;
        }

        private async Task HandleAddMaintenanceSubmit()
        {
            try
            {
                if (selectedInvoiceFile != null)
                {
                    var filePath = await FileService.UploadInvoiceAsync(selectedInvoiceFile);
                    newMaintenanceLog.InvoiceFilePath = filePath;
                }

                if (await MaintenanceService.AddMaintenanceLogAsync(Id, newMaintenanceLog))
                {
                    await LoadCar();
                    CloseAddMaintenanceModal();
                    
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Success",
                        Message = $"Maintenance log for {newMaintenanceLog.PartNumber} has been added successfully!"
                    });
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Failed to add maintenance log. Please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to add maintenance log: {ex.Message}"
                });
            }
        }

        private void EditMaintenanceLog(MaintenanceLog log)
        {
            // Navigate to edit page or show edit modal
            Console.WriteLine($"Edit maintenance log: {log.Id}");
        }

        private async Task DeleteMaintenanceLog(string logId)
        {
            try
            {
                if (await MaintenanceService.DeleteMaintenanceLogAsync(Id, logId))
                {
                    await LoadCar();
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Success",
                        Message = "Maintenance log has been deleted successfully!"
                    });
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Failed to delete maintenance log. Please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to delete maintenance log: {ex.Message}"
                });
            }
        }

        private async Task DownloadInvoice(string filePath)
        {
            try
            {
                var fileBytes = await FileService.DownloadInvoiceAsync(filePath);
                if (fileBytes != null)
                {
                    // In a real app, you'd trigger a file download
                    Console.WriteLine($"Downloading invoice: {filePath}");
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Info,
                        Title = "Download Started",
                        Message = "Invoice download has been initiated."
                    });
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Invoice file not found or could not be downloaded."
                    });
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to download invoice: {ex.Message}"
                });
            }
        }

        private async Task DeleteCar()
        {
            try
            {
                if (car == null) return;

                if (await CarService.DeleteCarAsync(Id))
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Success",
                        Message = $"Car {car.Year} {car.Make} {car.Model} has been deleted successfully!"
                    });
                    Navigation.NavigateTo("/cars");
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Failed to delete car. Please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to delete car: {ex.Message}"
                });
            }
        }
    }
}
