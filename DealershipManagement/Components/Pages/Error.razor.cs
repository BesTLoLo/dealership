using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace DealershipManagement.Components.Pages
{
    public partial class Error
    {
        [CascadingParameter]
        private HttpContext? HttpContext { get; set; }

        [Inject]
        private ToastService ToastService { get; set; } = default!;

        private string? RequestId { get; set; }
        private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        protected override void OnInitialized()
        {
            RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
            
            ToastService.Notify(new ToastMessage
            {
                Type = ToastType.Danger,
                Title = "Application Error",
                Message = "An error occurred while processing your request. Please try again or contact support if the problem persists."
            });
        }
    }
}
