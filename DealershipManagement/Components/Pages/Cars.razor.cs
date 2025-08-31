using BlazorBootstrap;
using DealershipManagement.Models;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Components;

namespace DealershipManagement.Components.Pages
{
    public partial class Cars
    {
        [Inject]
        private ICarService CarService { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private ToastService ToastService { get; set; } = default!;

        private List<Car>? cars;
        private bool showAddModal = false;
        private Car newCar = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadCars();
        }

        private async Task LoadCars()
        {
            cars = (await CarService.GetAllCarsAsync()).ToList();
        }

        private void ShowAddCarModal()
        {
            Console.WriteLine("ShowAddCarModal called"); // Debug line
            newCar = new Car();
            showAddModal = true;
        }

        private void CloseAddCarModal()
        {
            showAddModal = false;
        }

        private async Task HandleAddCarSubmit()
        {
            try
            {
                await CarService.AddCarAsync(newCar);
                await LoadCars();
                CloseAddCarModal();
                
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Success,
                    Title = "Success",
                    Message = $"Car {newCar.Year} {newCar.Make} {newCar.Model} has been added successfully!"
                });
            }
            catch (Exception ex)
            {
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Error",
                    Message = $"Failed to add car: {ex.Message}"
                });
            }
        }

        private void ViewCar(string id)
        {
            Navigation.NavigateTo($"/car/{id}");
        }

        private void EditCar(string id)
        {
            Navigation.NavigateTo($"/car/{id}/edit");
        }

        private void SellCar(string id)
        {
            Navigation.NavigateTo($"/car/{id}/sell");
        }

        private async Task DeleteCar(string id)
        {
            try
            {
                var car = cars?.FirstOrDefault(c => c.Id == id);
                if (car == null)
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Warning,
                        Title = "Warning",
                        Message = "Car not found!"
                    });
                    return;
                }

                if (await CarService.DeleteCarAsync(id))
                {
                    await LoadCars();
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Success",
                        Message = $"Car {car.Year} {car.Make} {car.Model} has been deleted successfully!"
                    });
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
