using FITTRACK.Data;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using FITTRACK.ViewModels;
using FITTRACK.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FITTRACK;


public partial class App : Application
{
    private ServiceProvider _serviceProvider;

    public App()
    {
        _serviceProvider = InstantiateServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
        base.OnStartup(e);
    }

    private ServiceProvider InstantiateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<MainWindow>(provider => new MainWindow()
        {
            DataContext = provider.GetService<MainViewModel>()
        });


        services.AddSingleton<MainViewModel>();
        services.AddSingleton<SignInViewModel>();
        services.AddSingleton<SignUpViewModel>();
        services.AddSingleton<UserViewModel>();
        services.AddSingleton<ResetPasswordViewModel>();
        services.AddSingleton<UserProfileViewModel>();
        services.AddSingleton<WorkoutsViewModel>();
        services.AddSingleton<AddWorkoutWindowViewMode>();
        services.AddSingleton<AdminWorkoutsViewModel>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDataService, InMemoryDataService>();
        services.AddSingleton<DataContext>();
        services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

        return services.BuildServiceProvider();
    }
}
