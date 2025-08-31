using BlazorBootstrap;
using DealershipManagement.Models;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace DealershipManagement.Components.Pages
{
    public partial class CarSell
    {
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [Inject]
        private ICarService CarService { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ToastService ToastService { get; set; } = default!;

        private Car? car;
        private SaleInfo saleInfo = new();
        private bool isSubmitting = false;

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
                    return;
                }

                if (car.IsSold)
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Car Already Sold",
                        Message = "This car has already been sold."
                    });
                    Navigation.NavigateTo($"/car/{car.Id}");
                    return;
                }

                // Pre-populate sale info
                saleInfo.SellPrice = car.BuyPrice > 0 ? car.BuyPrice : 0;
                saleInfo.SellDate = DateTime.Today;
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

        private async Task HandleSubmit()
        {
            if (car == null) return;

            isSubmitting = true;
            try
            {
                var success = await CarService.SellCarAsync(Id, saleInfo.SellPrice, saleInfo.SellDate);
                if (success)
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Sale Completed!",
                        Message = $"Car {car.Year} {car.Make} {car.Model} has been sold successfully for {saleInfo.SellPrice.ToString("C")}!"
                    });
                    Navigation.NavigateTo($"/car/{car.Id}");
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Failed to complete sale. Please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to complete sale: {ex.Message}"
                });
            }
            finally
            {
                isSubmitting = false;
            }
        }

        public class SaleInfo
        {
            [Required]
            [Range(0.01, double.MaxValue, ErrorMessage = "Sale price must be greater than 0")]
            public decimal SellPrice { get; set; }

            [Required]
            public DateTime SellDate { get; set; }
        }
    }
}
