using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TruckingTmsMaui.App.ViewModels;
using TruckingTmsMaui.App.Pages;
using TruckingTmsMaui.Core.Interfaces;
using TruckingTmsMaui.Data.Context;
using TruckingTmsMaui.Data.Repositories;
using TruckingTmsMaui.Data.Seeding;

namespace TruckingTmsMaui.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    // Add a third-party font for icons, e.g., Font Awesome
                    fonts.AddFont("FASolid.otf", "FontAwesomeSolid");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            
            // Register Data Services (DbContext and Repositories)
            // Use AddSingleton for DbContextOptions, as configuration is platform-independent
            builder.Services.AddSingleton<TruckingTmsDbContext>();
            builder.Services.AddSingleton<IDataService<TruckingTmsMaui.Core.Entities.ClientProfile>, ClientProfileService>();
            builder.Services.AddSingleton<IDataService<TruckingTmsMaui.Core.Entities.Customer>, CustomerService>();
            builder.Services.AddSingleton<IDriverService, DriverService>();
            builder.Services.AddSingleton<IDataService<TruckingTmsMaui.Core.Entities.Vehicle>, VehicleService>();
            builder.Services.AddSingleton<IJobService, JobService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IDocumentService, DocumentService>();
            builder.Services.AddSingleton<ISeedingService, SeedingService>();

            // Register ViewModels (Transient or Singleton based on usage - Singleton for Shell/Global state, Transient for Pages)
            // We register the main global state managers as singletons:
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();

            // Transient ViewModels for pages
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<JobsListViewModel>();
            builder.Services.AddTransient<JobDetailViewModel>();
            builder.Services.AddTransient<CustomersListViewModel>();
            builder.Services.AddTransient<ClientProfileDetailViewModel>();
            builder.Services.AddTransient<DispatchBoardViewModel>();
            builder.Services.AddTransient<DriverHomeViewModel>(); // Mobile driver app home

            // Register Pages for navigation (Transient is standard)
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<JobsListPage>();
            builder.Services.AddTransient<JobDetailPage>();
            builder.Services.AddTransient<CustomersListPage>();
            builder.Services.AddTransient<ClientProfileDetailPage>();
            builder.Services.AddTransient<DispatchBoardPage>();
            builder.Services.AddTransient<DriverHomePage>(); // Mobile view

            return builder.Build();
        }
    }
}