using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace DealershipManagement.Components.Pages
{
    public partial class Home
    {
        [Inject]
        private ToastService ToastService { get; set; } = default!;

        protected override void OnInitialized()
        {
            //ToastService.Notify(new ToastMessage
            //{
            //    Type = ToastType.Success,
            //    Title = "Welcome!",
            //    Message = "Welcome to MA Cars Management System. You can now manage your car inventory with ease!"
            //});
        }
    }
}
