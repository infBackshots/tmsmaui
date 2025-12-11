using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Shared.Mvvm;
using TruckingTmsMaui.App.Pages;

namespace TruckingTmsMaui.App.ViewModels
{
    public partial class CustomersListViewModel : BaseViewModel
    {
        private readonly IDataService<Customer> _customerService;

        [ObservableProperty]
        private ObservableCollection<Customer> _customers = new();
        
        [ObservableProperty]
        private Customer? _selectedCustomer;

        public CustomersListViewModel(IDataService<Customer> customerService)
        {
            _customerService = customerService;
            Title = "Customer Accounts";
            PropertyChanged += OnSelectedCustomerChanged;
        }

        public override async Task InitializeAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var allCustomers = await _customerService.GetAllAsync();
                Customers.Clear();
                foreach (var customer in allCustomers)
                {
                    Customers.Add(customer);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        private async void OnSelectedCustomerChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCustomer) && SelectedCustomer != null)
            {
                // In a real app, this would navigate to a Customer Detail Page
                await Shell.Current.DisplayAlert("Customer Selected", $"Navigating to detail for {SelectedCustomer.Name}", "OK");
                SelectedCustomer = null;
            }
        }
        
        [RelayCommand]
        private async Task CreateNewCustomer()
        {
            await Shell.Current.DisplayAlert("Feature Stub", "Opening form to create a new customer account.", "OK");
        }
        
        [RelayCommand]
        private async Task ViewClientProfile()
        {
             // Stub: In the real app, this would route to a Client Profile list/detail
             await Shell.Current.GoToAsync($"{nameof(ClientProfileDetailPage)}?ClientProfileId=1");
        }
    }
}