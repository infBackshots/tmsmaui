using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    public partial class JobDetailViewModel : BaseViewModel
    {
        private readonly IJobService _jobService;
        private readonly IDriverService _driverService;
        private readonly IDocumentService _documentService;
        private readonly IDataService<ClientProfile> _clientProfileService;

        [ObservableProperty]
        private Job _job = new();
        
        [ObservableProperty]
        private ObservableCollection<Driver> _availableDrivers = new();
        
        [ObservableProperty]
        private ObservableCollection<ClientProfile> _availableClientProfiles = new();
        
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedDriver))]
        private Driver? _selectedDriver;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLeaser))]
        private ClientProfile? _selectedClientProfile;
        
        public List<BillingTerms> BillingTermsOptions { get; } = Enum.GetValues(typeof(BillingTerms)).Cast<BillingTerms>().ToList();
        
        public bool IsNewJob => Job.Id == 0;
        public bool IsLeaser => Job.IsLeaser;

        public JobDetailViewModel(
            IJobService jobService, 
            IDriverService driverService, 
            IDocumentService documentService,
            IDataService<ClientProfile> clientProfileService)
        {
            _jobService = jobService;
            _driverService = driverService;
            _documentService = documentService;
            _clientProfileService = clientProfileService;
            
            Job.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Job.IsLeaser))
                {
                    OnPropertyChanged(nameof(IsLeaser));
                }
            };
        }
        
        public async Task LoadJobAsync(int jobId)
        {
            if (IsBusy) return;
            IsBusy = true;
            
            try
            {
                // Load common resources
                var drivers = await _driverService.GetAllAsync();
                AvailableDrivers.Clear();
                foreach (var driver in drivers) AvailableDrivers.Add(driver);

                var profiles = await _clientProfileService.GetAllAsync();
                AvailableClientProfiles.Clear();
                foreach (var profile in profiles) AvailableClientProfiles.Add(profile);

                if (jobId == 0)
                {
                    // New Job
                    Job = new Job { JobNumber = $"NEW-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}" };
                    Title = "Create New Job";
                }
                else
                {
                    // Existing Job
                    var jobEntity = await _jobService.GetByIdAsync(jobId);
                    if (jobEntity != null)
                    {
                        Job = jobEntity;
                        Title = $"Job: {Job.JobNumber}";
                        
                        SelectedDriver = AvailableDrivers.FirstOrDefault(d => d.Id == Job.DriverId);
                        SelectedClientProfile = AvailableClientProfiles.FirstOrDefault(cp => cp.Id == Job.ClientProfileId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading job data: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Could not load job details.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        partial void OnSelectedDriverChanged(Driver? value)
        {
            if (value != null)
            {
                Job.DriverId = value.Id;
                Job.DriverPhoneNumber = value.PhoneNumber;
            }
            else
            {
                Job.DriverId = null;
                Job.DriverPhoneNumber = string.Empty;
            }
        }
        
        partial void OnSelectedClientProfileChanged(ClientProfile? value)
        {
            if (value != null)
            {
                Job.ClientProfileId = value.Id;
                // Prefill notes/instructions from Client Profile
                Job.NotesSpecialInstructions += $"\n--- Client Profile Instructions from {value.DisplayName} ---\n{value.ServiceInstructions}";
            }
            else
            {
                Job.ClientProfileId = null;
            }
        }

        [RelayCommand]
        private async Task SaveJob()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                if (IsNewJob)
                {
                    await _jobService.AddAsync(Job);
                }
                else
                {
                    await _jobService.UpdateAsync(Job);
                }
                
                // Show success toast (using DisplayAlert as a placeholder for a Toast service)
                await Shell.Current.DisplayAlert("Success", $"Job {Job.JobNumber} saved successfully.", "OK");
                await Shell.Current.GoToAsync(".."); // Navigate back to list
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save job: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        [RelayCommand]
        private async Task UpdateStatus(JobStatus newStatus)
        {
            if (Job.Id == 0) return;
            
            Job.Status = newStatus;
            await SaveJobCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task DeleteJob()
        {
            if (Job.Id == 0) return;
            
            var confirmed = await Shell.Current.DisplayAlert("Confirm Delete", $"Are you sure you want to delete Job {Job.JobNumber}?", "Yes", "No");
            if (confirmed)
            {
                await _jobService.DeleteAsync(Job.Id);
                await Shell.Current.DisplayAlert("Success", $"Job {Job.JobNumber} deleted.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }
        
        [RelayCommand]
        private async Task UploadDocument(DocumentType type)
        {
            if (Job.Id == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Please save the job before attaching documents.", "OK");
                return;
            }

            try
            {
                // Use MAUI's FilePicker to get a file path
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    // Only allow PDF or images for now
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.item" } },
                        { DevicePlatform.Android, new[] { "*/*" } },
                        { DevicePlatform.WinUI, new[] { ".pdf", ".jpg", ".png" } },
                        { DevicePlatform.macOS, new[] { ".pdf", ".jpg", ".png" } },
                    }),
                    PickerTitle = $"Select {type} Document"
                });

                if (result != null)
                {
                    var attachment = await _documentService.AttachDocumentToJobAsync(Job.Id, type, result.FullPath);
                    
                    if (attachment != null)
                    {
                        Job.DocumentAttachments.Add(attachment);
                        // Trigger rate confirmation checkbox update if type matches
                        if (type == DocumentType.RateConfirmation) Job.RateConfirmationUploaded = true;
                        
                        await Shell.Current.DisplayAlert("Success", $"{type} uploaded successfully.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"File upload failed: {ex.Message}", "OK");
            }
        }
        
        [RelayCommand]
        private async Task ViewClientProfile()
        {
            if (SelectedClientProfile != null)
            {
                await Shell.Current.GoToAsync($"{nameof(ClientProfileDetailPage)}?ClientProfileId={SelectedClientProfile.Id}");
            }
        }
    }
}