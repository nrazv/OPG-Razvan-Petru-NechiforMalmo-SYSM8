using FITTRACK.AppExceptions;
using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;


namespace FITTRACK.ViewModels;

public class SignInViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    private IDataService _dataService;
    private string _userName = string.Empty;
    private string _password = string.Empty;

    public RelayCommand NavigateToSignUp { get; set; }
    public RelayCommand LoginCommand { get; set; }

    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
            Validate(nameof(UserName), value);
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            Validate(nameof(Password), value);
        }
    }

    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }
    }

    public SignInViewModel(INavigationService navigationService, IDataService dataService)
    {
        NavigationService = navigationService;
        _dataService = dataService;
        NavigateToSignUp = _createNavigateToSignUp();
        LoginCommand = _createLoginCommand();
    }

    private void _login()
    {
        try
        {
            var credentials = new UserCredentials();
            credentials.UserName = UserName;
            credentials.Password = Password;

            var user = _dataService.Login(credentials);
        }
        catch (UserNotFound e)
        {
            Errors["UserName"] = new List<string>() { e.Message };
            NotifyErrorsChanged("UserName");
        }
        catch (InvalidCredentials e)
        {
            Errors["Password"] = new List<string>() { e.Message };
            NotifyErrorsChanged("Password");
        }
    }

    private void clearForm()
    {
        UserName = string.Empty;
        Password = string.Empty;
        Errors.Remove(nameof(UserName));
        Errors.Remove(nameof(Password));
        NotifyErrorsChanged(nameof(UserName));
        NotifyErrorsChanged(nameof(Password));
    }

    private RelayCommand _createNavigateToSignUp()
    {
        return new RelayCommand(execute: o => { NavigationService.NavigateTo<SignUpViewModel>(); clearForm(); },
            canExecute: o => true);
    }

    private RelayCommand _createLoginCommand()
    {
        return new RelayCommand(
            execute: _ =>
            {
                _login();
            },
            canExecute: _ => Validator.TryValidateObject(this, new ValidationContext(this), null) && !Errors.Any());
    }

}
