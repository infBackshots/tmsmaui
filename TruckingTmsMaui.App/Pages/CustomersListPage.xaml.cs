using TruckingTmsMaui.App.ViewModels;

namespace TruckingTmsMaui.App.Pages
{
    public partial class CustomersListPage : ContentPage
    {
        public CustomersListPage(CustomersListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is CustomersListViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }
    }
}