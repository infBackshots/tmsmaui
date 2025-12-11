using TruckingTmsMaui.App.ViewModels;

namespace TruckingTmsMaui.App.Pages
{
    // The Route property JobId is set by the Shell navigation
    [QueryProperty(nameof(JobId), "JobId")]
    public partial class JobDetailPage : ContentPage
    {
        private readonly JobDetailViewModel _viewModel;
        
        public int JobId { get; set; }

        public JobDetailPage(JobDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Pass the ID to the ViewModel and trigger initialization
            await _viewModel.LoadJobAsync(JobId);
        }
    }
}