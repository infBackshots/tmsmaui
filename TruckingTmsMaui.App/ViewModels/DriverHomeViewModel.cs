using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    // 5) Mobile (Driver App – MAUI Mobile Views)
    public partial class DriverHomeViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private Job _nextJob = new Job(); // Simplified: just show the first job in the list as the "next"

        public DriverHomeViewModel(IJobService jobService, IAuthService authService)
        {
            _jobService = jobService;
            _authService = authService;
            Title = "Driver Home";
            
            // Note: In a real app, this would filter by the logged-in driver ID.
        }

        public override async Task InitializeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            
            try
            {
                var allJobs = await _jobService.GetAllAsync();
                var driverId = _authService.GetCurrentUser()?.Id;
                
                // Find the next job for the logged-in driver (simplification)
                var nextJob = allJobs.FirstOrDefault(j => j.DriverId == driverId && j.Status == Enums.JobStatus.Scheduled);
                if (nextJob != null)
                {
                    NextJob = nextJob;
                }
                else
                {
                    NextJob = new Job { JobName = "No active loads assigned." };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading driver home data: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        [RelayCommand]
        private async Task StartTrip()
        {
            // Placeholder for trip workflow navigation
            await Shell.Current.DisplayAlert("Trip Start", $"Starting trip workflow for job {NextJob.JobNumber}", "OK");
        }
        
        [RelayCommand]
        private async Task UploadPod()
        {
            // Placeholder for uploading POD/Ticket
            await Shell.Current.DisplayAlert("Upload", "Navigating to document upload for current load.", "OK");
        }
        
        [RelayCommand]
        private async Task HOS()
        {
            await Shell.Current.DisplayAlert("HOS", "Navigating to HOS clock and status screen.", "OK");
        }
        
        [RelayCommand]
        private async Task CheckCalls()
        {
            await Shell.Current.DisplayAlert("Check Calls", "Navigating to log check calls.", "OK");
        }
        
        [RelayCommand]
        private async Task ReportIssue()
        {
            await Shell.Current.DisplayAlert("Issue", "Navigating to incident/defect report form.", "OK");
        }
    }
}