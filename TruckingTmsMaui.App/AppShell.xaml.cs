using TruckingTmsMaui.App.Pages;
using TruckingTmsMaui.App.ViewModels;

namespace TruckingTmsMaui.App
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;

            // Register routes dynamically for navigation
            Routing.RegisterRoute(nameof(JobDetailPage), typeof(JobDetailPage));
            Routing.RegisterRoute(nameof(ClientProfileDetailPage), typeof(ClientProfileDetailPage));
        }
    }
}