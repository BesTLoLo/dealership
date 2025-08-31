using BlazorBootstrap;
using DealershipManagement.Models;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Components;

namespace DealershipManagement.Components.Pages
{
    public partial class CarEdit
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

        private async Task HandleSubmit()
        {
            if (car == null) return;

            isSubmitting = true;
            try
            {
                var success = await CarService.UpdateCarAsync(car);
                if (success)
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Success",
                        Message = $"Car {car.Year} {car.Make} {car.Model} has been updated successfully!"
                    });
                    Navigation.NavigateTo($"/car/{car.Id}");
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Failed to update car. Please try again."
                    });
                }
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to update car: {ex.Message}"
                });
            }
            finally
            {
                isSubmitting = false;
            }
        }
    }
}
