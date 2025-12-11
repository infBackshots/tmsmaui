using TruckingTmsMaui.App.ViewModels;
using TruckingTmsMaui.Shared.Constants;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.Pages
{
    public partial class JobsListPage : ContentPage
    {
        private readonly JobsListViewModel _viewModel;

        public JobsListPage(JobsListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }

        // Handle URL parameters for filtering (e.g., from Dashboard)
        protected override bool OnBackButtonPressed()
        {
            // Clear any pending filters before navigating back
            _viewModel.SelectedJobStatus = null;
            return base.OnBackButtonPressed();
        }

        private void OnStatusFilterChanged(object sender, EventArgs e)
        {
            // Trigger filtering when the picker selection changes
            _viewModel.FilterCommand.Execute(null);
        }
    }
    
    // Simple converter to extract city from a placeholder address string for UI display
    public class CityExtractorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string address)
            {
                // Assuming addresses are like "123 Main St, Salt Lake City, UT 84101"
                var parts = address.Split(',');
                if (parts.Length >= 2)
                {
                    return parts[1].Trim();
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Simple converter for Leaser checkmark
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isLeaser && isLeaser)
            {
                return Color.FromArgb("#F39C12"); // Orange for Leaser
            }
            return Color.FromArgb("#2ECC71"); // Green for regular
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}