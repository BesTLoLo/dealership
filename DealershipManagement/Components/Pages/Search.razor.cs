using BlazorBootstrap;
using DealershipManagement.Models;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DealershipManagement.Components.Pages
{
    public partial class Search
    {
        [Inject]
        private ICarService CarService { get; set; } = default!;

        [Inject]
        private ToastService ToastService { get; set; } = default!;

        private List<Car>? searchResults;
        private bool hasSearched = false;
        private bool showAdvancedFilters = false;
        private SearchFilters searchFilters = new();
        private long totalCount = 0;
        private int totalPages = 0;

        protected override void OnInitialized()
        {
            // Set default dates
            searchFilters.StartDate = DateTime.Today.AddMonths(-6);
            searchFilters.EndDate = DateTime.Today;
        }

        private async Task PerformSearch()
        {
            Console.WriteLine("PerformSearch called"); // Debug line
            hasSearched = true;
            searchResults = null;
            
            try
            {
                searchResults = (await CarService.SearchCarsAsync(searchFilters)).ToList();
                totalCount = await CarService.GetCarsCountAsync(searchFilters);
                totalPages = (int)Math.Ceiling((double)totalCount / searchFilters.PageSize);
                
                if (searchResults.Any())
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Success,
                        Title = "Search Complete",
                        Message = $"Found {totalCount} car(s) matching your criteria."
                    });
                }
                else
                {
                    ToastService.Notify(new ToastMessage
                    {
                        Type = ToastType.Info,
                        Title = "No Results",
                        Message = "No cars found matching your search criteria."
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search error: {ex.Message}"); // Debug line
                ToastService.Notify(new ToastMessage
                {
                    Type = ToastType.Danger,
                    Title = "Search Error",
                    Message = $"Failed to perform search: {ex.Message}"
                });
            }
        }

        private async void HandleSearch(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await PerformSearch();
            }
        }

        private void ShowAdvancedFilters()
        {
            Console.WriteLine("ShowAdvancedFilters called"); // Debug line
            showAdvancedFilters = !showAdvancedFilters;
        }

        private void ClearFilters()
        {
            Console.WriteLine("ClearFilters called"); // Debug line
            searchFilters = new SearchFilters
            {
                StartDate = DateTime.Today.AddMonths(-6),
                EndDate = DateTime.Today,
                Page = 1,
                PageSize = 10
            };
            
            ToastService.Notify(new ToastMessage
            {
                Type = ToastType.Info,
                Title = "Filters Cleared",
                Message = "All search filters have been reset to default values."
            });
        }

        private async Task ChangePage(int page)
        {
            searchFilters.Page = page;
            await PerformSearch();
        }
    }
}
