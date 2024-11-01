

using FITTRACK.AppExceptions;
using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.MVVM.Validation;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace FITTRACK.ViewModels;

public class SignUpViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly IDataService _dataService;
    private INavigationService _navigationService;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();


    private string _userName = string.Empty;
    private string _password = string.Empty;
    private string _passwordConfirmation = string.Empty;
    private string _country = string.Empty;
    private string _questionAnswer = string.Empty;
    private SecurityQuestion _question;

    public bool HasErrors => Errors.Any();
    public ObservableCollection<string> Countries { get; }
    public ObservableCollection<SecurityQuestion> SecurityQuestions { get; }
    public RelayCommand SignUpCommand { get; set; }
    public RelayCommand NavigateToSigIn { get; set; }
    public INavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; } }


    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    [Required(ErrorMessage = "Username is required")]
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            Validate(nameof(UserName), value);
            OnPropertyChanged();
        }
    }


    [PasswordValidation]
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            Validate(nameof(Password), value);
            OnPropertyChanged();
        }
    }

    [PasswordValidation]
    public string PasswordConfirmation
    {
        get => _passwordConfirmation;
        set
        {
            _passwordConfirmation = value;
            Validate(nameof(PasswordConfirmation), value);
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "A answer is required")]
    public string QuestionAnswer
    {
        get => _questionAnswer;
        set
        {
            _questionAnswer = value;
            Validate(nameof(QuestionAnswer), value);
            OnPropertyChanged();
        }
    }

    [Required(ErrorMessage = "Country is required")]
    public string Country
    {
        get => _country;
        set
        {
            _country = value;
            Validate(nameof(Country), value);
            OnPropertyChanged();
        }
    }

    [Required]
    public SecurityQuestion Question
    {
        get => _question;
        set
        {
            _question = value;
            OnPropertyChanged();
        }
    }


    public SignUpViewModel(IDataService dataService, INavigationService navigationService)
    {
        NavigationService = navigationService;
        _dataService = dataService;
        SignUpCommand = new RelayCommand(execute: signUp, canExecute: canSignUp);
        SecurityQuestions = new ObservableCollection<SecurityQuestion>(_dataService.GetSecurityQuestions());
        Countries = new ObservableCollection<string>(_dataService.Countries());
        NavigateToSigIn = new RelayCommand(execute: o => { NavigationService.NavigateTo<SignInViewModel>(); clearFormData(); },
            canExecute: o => true);
    }

    public new IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is null) return Enumerable.Empty<string>();
        if (Errors.ContainsKey(propertyName))
        {
            return Errors[propertyName];
        }
        else
        {
            return Enumerable.Empty<string>();
        }
    }

    // Custom implemantation of validate method of INotifyDataErrorInfo
    public new void Validate(string propertyName, object propertyValue)
    {
        // validate properties and add errors to Errors list
        var results = new List<ValidationResult>();
        Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

        if (results.Any())
        {
            var errors = new List<string>();
            results.ForEach(x => errors.Add(x.ErrorMessage ?? ""));

            // update errors for properties in Errors list 
            if (Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = errors;
            }

            if (!Errors.ContainsKey(propertyName))
            {

                Errors.Add(propertyName, errors);
            }


            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        else
        {
            // implement passwords match 
            if (propertyName.Equals("PasswordConfirmation"))
            {
                bool passwordMatch = Password.Equals(propertyValue);
                // remove errors of type PasswordConfirmation
                if (passwordMatch)
                {
                    Errors.Remove(propertyName);
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }

                // add errors to password PasswordConfirmation
                if (!passwordMatch)
                {
                    Errors[propertyName] = new List<string>() { "Passwords do not match" };
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                    return;
                }
            }

            Errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        OnPropertyChanged(nameof(Errors));
    }


    private bool canSignUp(object obj)
    {
        return Validator.TryValidateObject(this, new ValidationContext(this), null) && !Errors.Any();
    }

    private void signUp(object obj)
    {
        bool userCreated = false;
        var user = new User()
        {
            Id = Guid.NewGuid(),
            UserName = UserName,
            Password = Password,
            Country = Country,
            SecurityQuestion = new UserSecurityQuestion(Question, QuestionAnswer),
        };

        try
        {
            userCreated = _dataService.AddUser(user);
        }
        catch (UserTaken ex)
        {
            Errors[nameof(UserName)] = new List<string>() { "Username is taken" };
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(UserName)));
        }

        if (userCreated)
        {
            clearFormData();
            MessageBoxResult result = MessageBox.Show($"A new user with username {user.UserName} has been created.");
            if (result == MessageBoxResult.OK && userCreated)
            {
                NavigateToSigIn.Execute(this);
            }
        }
    }

    // clear fields data and errors
    private void clearFormData()
    {
        UserName = string.Empty;
        Password = string.Empty;
        PasswordConfirmation = string.Empty;
        QuestionAnswer = string.Empty;
        Country = null;
        Question = null;

        Errors.Remove(nameof(UserName));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(UserName)));
        Errors.Remove(nameof(Password));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Password)));
        Errors.Remove(nameof(PasswordConfirmation));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(PasswordConfirmation)));
        Errors.Remove(nameof(QuestionAnswer));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(QuestionAnswer)));
        Errors.Remove(nameof(Country));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Country)));
        Errors.Remove(nameof(Question));
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Question)));
    }

}
