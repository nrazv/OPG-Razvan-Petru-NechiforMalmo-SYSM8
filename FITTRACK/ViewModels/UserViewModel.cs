using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using FITTRACK.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace FITTRACK.ViewModels;
public class UserViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    private IDataService _dataService;

    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand NavigateToProfileCommand { get; set; }
    public RelayCommand NavigateToWorkoutsCommand { get; set; }
    public RelayCommand LogOutCommand { get; set; }


    public UserViewModel(INavigationService navigationService, IDataService dataService)
    {
        NavigationService = navigationService;
        NavigationService.NavigateToNestedView<WorkoutsViewModel>();
        _dataService = dataService;
        createNavigationCommands();
    }


    private void createNavigationCommands()
    {
        NavigateToProfileCommand = new RelayCommand(execute: e => NavigationService.NavigateToNestedView<UserProfileViewModel>(), canExecute: e => true);
        NavigateToWorkoutsCommand = new RelayCommand(execute: e => NavigationService.NavigateToNestedView<WorkoutsViewModel>(), canExecute: e => true);
        LogOutCommand = new RelayCommand(execute: logOut, canExecute: e => true);
    }

    private void logOut(object obj)
    {
        _dataService.LogOut();
        NavigationService.NavigateTo<SignInViewModel>();
    }
}

