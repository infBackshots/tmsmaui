using TruckingTmsMaui.App.ViewModels;

namespace TruckingTmsMaui.App.Pages
{
    // Receives ClientProfileId from navigation query string
    [QueryProperty(nameof(ClientProfileId), "ClientProfileId")]
    public partial class ClientProfileDetailPage : ContentPage
    {
        private readonly ClientProfileDetailViewModel _viewModel;
        
        public string ClientProfileId 
        { 
            // Set the property on the ViewModel via the binding context
            set => _viewModel.ClientProfileId = value; 
        }

        public ClientProfileDetailPage(ClientProfileDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        // Note: Initialization is handled inside the ViewModel property setter (ClientProfileId)
    }
}