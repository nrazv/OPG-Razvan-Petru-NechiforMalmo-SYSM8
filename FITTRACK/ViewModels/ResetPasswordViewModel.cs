

using FITTRACK.AppExceptions;
using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace FITTRACK.ViewModels;

public class ResetPasswordViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    private IDataService _dataService;
    private string _username;
    private string _securityQuestion;
    private Visibility _userFound;
    private Visibility _canReset = Visibility.Collapsed;
    private string _answre;
    private string _newPassword;
    public User User { get; set; }

    public Visibility UserFound
    {
        get => _userFound;
        set
        {
            _userFound = value;
            OnPropertyChanged();
        }
    }


    public Visibility CanReset
    {
        get => _canReset;
        set
        {
            _canReset = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public string UserName
    {
        get => _username;
        set
        {
            _username = value;
            Validate(nameof(UserName), value);
            OnPropertyChanged();
        }
    }

    public string SecurityQuestion
    {
        get => _securityQuestion;
        set
        {
            _securityQuestion = value;
            OnPropertyChanged();
        }
    }

    public string Answer
    {
        get => _answre;
        set
        {
            _answre = value;
            OnPropertyChanged();
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            _newPassword = value;
            OnPropertyChanged();
        }
    }


    public RelayCommand SearchUserCommand { get; set; }
    public RelayCommand ResetPasswordCommand { get; set; }
    public RelayCommand NavigateToSigIn { get; set; }

    public ResetPasswordViewModel(IDataService dataService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
        SearchUserCommand = _createSearchUserCommand();
        NavigateToSigIn = new RelayCommand(execute: o => _navigationService.NavigateTo<SignInViewModel>(),
         canExecute: o => true);
        ResetPasswordCommand = new RelayCommand(execute: e =>
        {
            resetPassword();
        }, canExecute: e => true);
    }

    private void resetPassword()
    {
        UserSecurityQuestion question = User.SecurityQuestion;

        if (string.Equals(question.Answer, Answer))
        {
            MessageBoxResult result = MessageBox.Show($"Your password has been reseted !");
            if (result == MessageBoxResult.OK)
            {
                User.Password = NewPassword;
                _navigationService.NavigateTo<SignInViewModel>();
            }

        }
        else
        {
            Errors["Answer"] = new List<string>() { "Answer don't match !" };
            NotifyErrorsChanged("Answer");
        }

    }


    private void searchUser()
    {
        try
        {
            User = _dataService.GetByUserName(UserName);
            UserSecurityQuestion question = User.SecurityQuestion;
            SecurityQuestion = question.Question.Question;
            UserFound = Visibility.Collapsed;

            if (SecurityQuestion is not null)
            {
                CanReset = Visibility.Visible;
            }

        }
        catch (UserNotFound e)
        {
            Errors["UserName"] = new List<string>() { e.Message };
            NotifyErrorsChanged("UserName");
        }
    }


    private RelayCommand _createSearchUserCommand()
    {
        return new RelayCommand(
            execute: _ =>
            {
                searchUser();
            },
            canExecute: _ => Validator.TryValidateObject(this, new ValidationContext(this), null) && !Errors.Any());
    }

}
