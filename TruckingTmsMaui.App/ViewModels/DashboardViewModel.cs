using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;
        
        public DashboardViewModel(IJobService jobService)
        {
            _jobService = jobService;
            Title = "Dashboard";
        }
        
        public override async Task InitializeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                // Example of loading data for a dashboard widget
                var totalJobs = (await _jobService.GetAllAsync()).Count();
                // This would be used to populate the widgets with real data
                Console.WriteLine($"Total Jobs Loaded: {totalJobs}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing dashboard: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToJobsList(JobStatus? status = null)
        {
            // Simple navigation to the Jobs list page, optionally filtering by status
            var query = status.HasValue ? $"?status={status.Value}" : string.Empty;
            await Shell.Current.GoToAsync($"//JobsList{query}");
        }
        
        [RelayCommand]
        private async Task GoToDispatch()
        {
            await Shell.Current.GoToAsync("//DispatchBoard");
        }
    }
}