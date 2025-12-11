using TruckingTmsMaui.App.ViewModels;

namespace TruckingTmsMaui.App.Pages
{
    public partial class DispatchBoardPage : ContentPage
    {
        public DispatchBoardPage(DispatchBoardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is DispatchBoardViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }
    }
}