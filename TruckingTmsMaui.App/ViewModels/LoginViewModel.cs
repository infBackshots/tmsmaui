using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.App.Pages;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly ISeedingService _seedingService;
        private readonly AppShellViewModel _appShellViewModel;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;
        
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public LoginViewModel(IAuthService authService, ISeedingService seedingService, AppShellViewModel appShellViewModel)
        {
            _authService = authService;
            _seedingService = seedingService;
            _appShellViewModel = appShellViewModel;
            Title = "Login";
            
            // Start the seeding process as soon as the app launches
            // In a production app, this would be a real migration/db setup check
            InitializeDatabaseCommand.Execute(null); 
        }
        
        [RelayCommand]
        private async Task InitializeDatabase()
        {
            IsBusy = true;
            try
            {
                // This ensures the DB is created and seeded with users on startup
                await _seedingService.SeedDatabaseAsync();
                ErrorMessage = "Database Initialized. Please login.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Database error: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task Login()
        {
            if (IsBusy) return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var user = await _authService.LoginAsync(Username, Password);

                if (user != null)
                {
                    // Successful login. Set up the main application shell.
                    var shell = Application.Current.Handler.MauiContext.Services.GetRequiredService<AppShell>();
                    
                    // Update the Shell VM with the new user's role
                    _appShellViewModel.UpdateRoleProperties();

                    Application.Current.MainPage = shell;

                    // Depending on the role, navigate to the specific starting page
                    if (_appShellViewModel.IsDriver)
                    {
                        await Shell.Current.GoToAsync("//DriverMobile/DriverHome");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//Dashboard");
                    }
                }
                else
                {
                    ErrorMessage = "Invalid username or password. Check credentials.";
                }
            }
            catch (Exception ex)
            {
                // Simple logging/error handling
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}