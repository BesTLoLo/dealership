using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace DealershipManagement.Components.Pages
{
    public partial class About
    {
        [Inject]
        private ToastService ToastService { get; set; } = default!;

        protected override void OnInitialized()
        {
            ToastService.Notify(new ToastMessage
            {
                Type = ToastType.Info,
                Title = "About Us",
                Message = "Learn more about MA Cars and our commitment to automotive excellence!"
            });
        }
    }
}
