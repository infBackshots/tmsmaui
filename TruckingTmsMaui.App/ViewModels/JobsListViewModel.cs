using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    // Apply QueryProperty to handle navigation parameters (e.g., from Dashboard)
    [QueryProperty(nameof(Status), "status")]
    public partial class JobsListViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;
        private List<Job> _allJobs = new();

        [ObservableProperty]
        private ObservableCollection<Job> _filteredJobs = new();
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasStatusFilter))]
        private JobStatus? _selectedJobStatus;
        
        [ObservableProperty]
        private string _searchText = string.Empty;

        // Property to receive the status from the URL query
        public string Status
        {
            set
            {
                if (Enum.TryParse<JobStatus>(value, out var status))
                {
                    SelectedJobStatus = status;
                    // Trigger initial filter when the property is set
                    FilterCommand.Execute(null); 
                }
            }
        }
        
        // Expose JobStatus enum values for the Picker
        public List<JobStatus> JobStatuses { get; } = Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>().ToList();

        public bool HasStatusFilter => SelectedJobStatus.HasValue;
        
        [ObservableProperty]
        private Job? _selectedJob;

        public JobsListViewModel(IJobService jobService)
        {
            _jobService = jobService;
            Title = "All Jobs / Orders";
            PropertyChanged += OnSelectedJobChanged;
        }

        public override async Task InitializeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            
            try
            {
                _allJobs = (await _jobService.GetAllAsync()).ToList();
                FilterCommand.Execute(null); // Apply initial filter/search
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading jobs: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        // This command is triggered by SearchBar or Picker changes
        [RelayCommand]
        private void Filter()
        {
            var results = _allJobs.AsEnumerable();

            // 1. Status Filter
            if (SelectedJobStatus.HasValue)
            {
                results = results.Where(j => j.Status == SelectedJobStatus.Value);
            }
            
            // 2. Search Text Filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.Trim().ToLower();
                results = results.Where(j => j.JobNumber.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             j.BrokerOrCustomerName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                             j.BolNumber.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            FilteredJobs.Clear();
            foreach (var job in results)
            {
                FilteredJobs.Add(job);
            }
        }

        private async void OnSelectedJobChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedJob) && SelectedJob != null)
            {
                await Shell.Current.GoToAsync($"{nameof(JobDetailPage)}?JobId={SelectedJob.Id}");
                SelectedJob = null; // Clear selection after navigation
            }
        }

        [RelayCommand]
        private async Task CreateNewJob()
        {
            // Navigate to Job Detail with ID 0 or null to indicate new job
            await Shell.Current.GoToAsync($"{nameof(JobDetailPage)}?JobId=0");
        }
    }
}