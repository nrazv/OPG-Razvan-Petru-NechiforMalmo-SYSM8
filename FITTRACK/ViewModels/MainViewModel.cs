using FITTRACK.MVVM;
using FITTRACK.Services.Navigation;
using System;

namespace FITTRACK.ViewModels;

public class MainViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }

    }

    public MainViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        NavigationService.NavigateTo<SignInViewModel>();
    }

}
