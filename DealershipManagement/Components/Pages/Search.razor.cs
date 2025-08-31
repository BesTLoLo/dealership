using BlazorBootstrap;
using DealershipManagement.Models;
using DealershipManagement.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Timers;

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
        private System.Timers.Timer? searchTimer;
        private bool isSearching = false;
        private List<string> searchSuggestions = new();
        private bool showSuggestions = false;

        protected override void OnInitialized()
        {
            // Set default dates
            searchFilters.StartDate = DateTime.Today.AddMonths(-6);
            searchFilters.EndDate = DateTime.Today;
            
            // Initialize search timer for debouncing
            searchTimer = new System.Timers.Timer(500); // 500ms delay
            searchTimer.Elapsed += async (sender, e) => await PerformSearchAsync();
            searchTimer.AutoReset = false;
        }

        public void Dispose()
        {
            searchTimer?.Dispose();
        }

        private async Task PerformSearch()
        {
            // Reset timer for debounced search
            searchTimer?.Stop();
            searchTimer?.Start();
            
            // Show suggestions immediately for short searches
            if (!string.IsNullOrEmpty(searchFilters.SearchTerm) && searchFilters.SearchTerm.Length <= 3)
            {
                await ShowSearchSuggestions();
            }
        }

        private async Task PerformSearchAsync()
        {
            await InvokeAsync(async () =>
            {
                Console.WriteLine("PerformSearch called"); // Debug line
                hasSearched = true;
                isSearching = true;
                searchResults = null;
                showSuggestions = false;
                
                StateHasChanged();
                
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
                finally
                {
                    isSearching = false;
                    StateHasChanged();
                }
            });
        }

        private async Task ShowSearchSuggestions()
        {
            if (string.IsNullOrEmpty(searchFilters.SearchTerm) || searchFilters.SearchTerm.Length < 2)
            {
                showSuggestions = false;
                searchSuggestions.Clear();
                return;
            }

            try
            {
                // Get suggestions from the service
                var suggestions = await CarService.GetSearchSuggestionsAsync(searchFilters.SearchTerm);
                searchSuggestions = suggestions.Take(5).ToList();
                showSuggestions = searchSuggestions.Any();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting suggestions: {ex.Message}");
                showSuggestions = false;
            }
        }

        private void SelectSuggestion(string suggestion)
        {
            searchFilters.SearchTerm = suggestion;
            showSuggestions = false;
            PerformSearch().ConfigureAwait(false);
        }

        private async void HandleSearch(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                searchTimer?.Stop();
                await PerformSearchAsync();
            }
            else if (e.Key == "Escape")
            {
                showSuggestions = false;
            }
            else
            {
                // Trigger search suggestions
                await ShowSearchSuggestions();
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
            
            searchResults = null;
            hasSearched = false;
            showSuggestions = false;
            searchSuggestions.Clear();
            
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
            await PerformSearchAsync();
        }

        private void OnSearchTermChanged(ChangeEventArgs e)
        {
            var value = e.Value?.ToString() ?? "";
            searchFilters.SearchTerm = value;
            if (!string.IsNullOrEmpty(value))
            {
                PerformSearch().ConfigureAwait(false);
            }
            else
            {
                showSuggestions = false;
                searchResults = null;
                hasSearched = false;
            }
        }

        private void OnSuggestionHover(string suggestion)
        {
            // This method can be used for additional hover effects if needed
            // Currently just a placeholder for the UI event binding
        }
    }
}
