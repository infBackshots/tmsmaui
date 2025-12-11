using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Enums;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.App.Pages;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        
        [ObservableProperty]
        private UserRole _currentUserRole = UserRole.Driver;

        public string UserRoleText => CurrentUserRole.ToString();

        // Role-based visibility properties
        public bool IsAdmin => CurrentUserRole == UserRole.Admin;
        public bool IsDispatcher => CurrentUserRole == UserRole.Dispatcher;
        public bool IsFinance => CurrentUserRole == UserRole.Finance;
        public bool IsFleetOps => CurrentUserRole == UserRole.FleetOps;
        public bool IsDriver => CurrentUserRole == UserRole.Driver;

        // Composite roles
        public bool IsOperationalUser => IsAdmin || IsDispatcher || IsFleetOps || IsFinance;
        public bool IsDispatcherOrAdmin => IsAdmin || IsDispatcher;
        public bool IsFinanceOrAdmin => IsAdmin || IsFinance;
        public bool IsFleetOpsOrAdmin => IsAdmin || IsFleetOps;

        public AppShellViewModel(IAuthService authService)
        {
            _authService = authService;
            UpdateRoleProperties(); // Initial check
        }
        
        // This is called by the LoginPage once login succeeds
        public void UpdateRoleProperties()
        {
            CurrentUserRole = _authService.GetCurrentUserRole();
            
            // Notify all role-based properties have changed
            OnPropertyChanged(nameof(CurrentUserRole));
            OnPropertyChanged(nameof(UserRoleText));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsDispatcher));
            OnPropertyChanged(nameof(IsFinance));
            OnPropertyChanged(nameof(IsFleetOps));
            OnPropertyChanged(nameof(IsDriver));
            OnPropertyChanged(nameof(IsOperationalUser));
            OnPropertyChanged(nameof(IsDispatcherOrAdmin));
            OnPropertyChanged(nameof(IsFinanceOrAdmin));
            OnPropertyChanged(nameof(IsFleetOpsOrAdmin));
        }

        [RelayCommand]
        private async Task Logout()
        {
            // Simple logout logic: clear current user and navigate back to login
            // In a real app, this would also clear any persistent tokens.
            await Shell.Current.GoToAsync("//LoginPage");
            
            // Note: The App.xaml.cs sets the initial MainPage to LoginPage.
            // When we log out, we replace the Shell with the LoginPage again.
            Application.Current.MainPage = new LoginPage(); 
        }
    }
}