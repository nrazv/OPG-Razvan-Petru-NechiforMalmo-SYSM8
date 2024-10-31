using FITTRACK.AppExceptions;
using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using static System.Runtime.CompilerServices.RuntimeHelpers;


namespace FITTRACK.ViewModels;

public class SignInViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    private IDataService _dataService;
    private string _userName = string.Empty;
    private string _password = string.Empty;
    private int _twfCode;
    private int _userTWFCode;
    private Visibility twfEnabled = Visibility.Collapsed;
    private Visibility twfModeOff = Visibility.Visible;

    public Visibility TWFEnabled
    {
        get => twfEnabled;
        set
        {
            twfEnabled = value;
            OnPropertyChanged();
        }
    }

    public Visibility TWFModeOff
    {
        get => twfModeOff;
        set
        {
            twfModeOff = value;
            OnPropertyChanged();
        }
    }

    public int TWFCode
    {
        get => _twfCode;
        set
        {
            _twfCode = value;
            OnPropertyChanged();
        }
    }

    public int UserTWFCode
    {
        get => _userTWFCode;
        set
        {
            _userTWFCode = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand NavigateToSignUp { get; set; }
    public RelayCommand LoginCommand { get; set; }
    public RelayCommand ForgotPasswordCommand { get; set; }
    public RelayCommand TWFCommand { get; set; }


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
        ForgotPasswordCommand = _createForgotPasswordCommand();
        TWFCommand = new RelayCommand(execute: e => { verify2WF(); }, canExecute: e => true);
    }

    private void _login()
    {
        try
        {
            var credentials = new UserCredentials();
            credentials.UserName = UserName;
            credentials.Password = Password;

            var user = _dataService.Login(credentials);
            if (user is not null)
            {
                TWFModeOff = Visibility.Collapsed;
                TWFEnabled = Visibility.Visible;
                twfAutentication();
            }
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

    private RelayCommand _createForgotPasswordCommand()
    {
        return new RelayCommand(
            execute: _ =>
            {
                NavigationService.NavigateTo<ResetPasswordViewModel>();
            },
            canExecute: _ => true);
    }


    private void twfAutentication()
    {
        TWFCode = generateCode();
        displayCode(TWFCode);
    }

    private void verify2WF()
    {
        if (UserTWFCode == TWFCode)
        {
            Errors.Clear();


            TWFModeOff = Visibility.Visible;
            TWFEnabled = Visibility.Collapsed;
            UserTWFCode = 0;
            NavigationService.NavigateTo<UserViewModel>();
        }
        else
        {
            Errors["UserTWFCode"] = new List<string>() { "Invalid code" };
            NotifyErrorsChanged("UserTWFCode");
        }
    }



    private void displayCode(int twfCode)
    {
        string messageBoxText = $"2FA Code: {twfCode}";
        string caption = "2FA Code";
        MessageBoxImage icon = MessageBoxImage.Information;
        MessageBoxResult result = MessageBox.Show(messageBoxText, caption);
    }

    private int generateCode()
    {
        Random random = new Random();
        return random.Next(100000, 1000000);
    }
}
