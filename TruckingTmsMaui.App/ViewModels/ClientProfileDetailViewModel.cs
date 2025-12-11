using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TruckingTmsMaui.Core.Entities;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Shared.Mvvm;

namespace TruckingTmsMaui.App.ViewModels
{
    [QueryProperty(nameof(ClientProfileId), "ClientProfileId")]
    public partial class ClientProfileDetailViewModel : BaseViewModel
    {
        private readonly IDataService<ClientProfile> _clientProfileService;

        [ObservableProperty]
        private ClientProfile _clientProfile = new();
        
        public string ClientProfileId
        {
            set => LoadProfileAsync(int.Parse(value)).Wait();
        }

        public ClientProfileDetailViewModel(IDataService<ClientProfile> clientProfileService)
        {
            _clientProfileService = clientProfileService;
            Title = "Client Profile Detail";
        }
        
        private async Task LoadProfileAsync(int id)
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var profile = await _clientProfileService.GetByIdAsync(id);
                if (profile != null)
                {
                    ClientProfile = profile;
                    Title = $"{profile.DisplayName} Profile";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveProfile()
        {
            await _clientProfileService.UpdateAsync(ClientProfile);
            await Shell.Current.DisplayAlert("Success", "Client Profile saved.", "OK");
        }
    }
}