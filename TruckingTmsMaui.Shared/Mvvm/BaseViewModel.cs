using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace TruckingTmsMaui.Shared.Mvvm
{
    // Base class for all ViewModels, providing ObservableObject functionality
    // and basic Command support.
    public abstract partial class BaseViewModel : ObservableObject, INotifyPropertyChanged
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        public bool IsNotBusy => !IsBusy;

        [ObservableProperty]
        string title = string.Empty;

        // Command to handle initialization logic for pages
        // This is a common pattern for loading data asynchronously in MAUI pages.
        [RelayCommand]
        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }
    }
}