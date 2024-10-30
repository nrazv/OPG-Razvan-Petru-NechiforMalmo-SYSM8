using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITTRACK.ViewModels;

public class UserProfileViewModel : ViewModelBase
{
    private InMemoryDataService _dataService;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();
    public bool HasErrors => Errors.Any();
    public ObservableCollection<string> Countries { get; }
    public ObservableCollection<SecurityQuestion> SecurityQuestions { get; }
    private User _authenticatedUser;

    private bool _editInfoOff = true;
    private string _userName = string.Empty;
    private string _password = string.Empty;
    private string _passwordConfirmation = string.Empty;
    private string _country = string.Empty;
    private string _questionAnswer = string.Empty;
    private SecurityQuestion _question;

    public bool EditInfo { get => _editInfoOff; set { _editInfoOff = value; OnPropertyChanged(); } }
    public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(); } }
    public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
    public string PasswordConfirmation { get => _passwordConfirmation; set { _passwordConfirmation = value; OnPropertyChanged(); } }


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





    public UserProfileViewModel(IDataService dataService)
    {
        _dataService = (InMemoryDataService)dataService;
        _authenticatedUser = _dataService.AuthenticatedUser;
        SecurityQuestions = new ObservableCollection<SecurityQuestion>(_dataService.GetSecurityQuestions());
        Countries = new ObservableCollection<string>(_dataService.Countries());
        setUserData();
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

    private void setUserData()
    {
        _userName = _authenticatedUser.UserName;
        _password = _authenticatedUser.Password;
        _country = _authenticatedUser.Country;
        _question = _authenticatedUser.SecurityQuestion.Question;
        _questionAnswer = _authenticatedUser.SecurityQuestion.Answer;
    }

    private void saveChanges()
    {

    }

}
