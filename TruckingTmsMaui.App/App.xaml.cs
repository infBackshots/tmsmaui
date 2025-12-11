using TruckingTmsMaui.App.Pages;

namespace TruckingTmsMaui.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            // Set the main page to the LoginPage initially
            MainPage = new LoginPage(); 
        }
    }
}