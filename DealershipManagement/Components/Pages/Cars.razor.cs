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

        private async Task<GridDataProviderResult<Car>> CarsDataProvider(GridDataProviderRequest<Car> request)
        {
            string sortString = "";
            SortDirection sortDirection = SortDirection.None;

            if (request.Sorting is not null && request.Sorting.Any())
            {
                // Note: Multi column sorting is not supported at this moment
                sortString = request.Sorting.FirstOrDefault()!.SortString;
                sortDirection = request.Sorting.FirstOrDefault()!.SortDirection;
            }

            // For now, we'll use the existing cars list and apply basic filtering/sorting
            // In a real application, you'd want to implement proper database-level filtering and pagination
            var filteredCars = cars ?? new List<Car>();

            // Apply filters if any
            if (request.Filters is not null && request.Filters.Any())
            {
                foreach (var filter in request.Filters)
                {
                    if (!string.IsNullOrEmpty(filter.Value))
                    {
                        filteredCars = filteredCars.Where(car =>
                        {
                            var propertyValue = GetPropertyValue(car, filter.PropertyName);
                            return propertyValue?.ToString()?.Contains(filter.Value, StringComparison.OrdinalIgnoreCase) == true;
                        }).ToList();
                    }
                }
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortString))
            {
                filteredCars = sortDirection == SortDirection.Ascending
                    ? filteredCars.OrderBy(car => GetPropertyValue(car, sortString)).ToList()
                    : filteredCars.OrderByDescending(car => GetPropertyValue(car, sortString)).ToList();
            }

            // Apply pagination
            var totalCount = filteredCars.Count;
            var pagedCars = filteredCars
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return await Task.FromResult(new GridDataProviderResult<Car> { Data = pagedCars, TotalCount = totalCount });
        }

        private object? GetPropertyValue(Car car, string propertyName)
        {
            return propertyName switch
            {
                "VIN" => car.VIN,
                "StockNumber" => car.StockNumber,
                "Make" => car.Make,
                "Model" => car.Model,
                "Year" => car.Year,
                "Status" => car.Status,
                "FinalBuyPrice" => car.FinalBuyPrice,
                "FinalSellPrice" => car.FinalSellPrice,
                _ => null
            };
        }
    }
}
