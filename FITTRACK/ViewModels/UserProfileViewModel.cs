using FITTRACK.AppExceptions;
using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.MVVM.Validation;
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
using System.Windows;

namespace FITTRACK.ViewModels;

public class UserProfileViewModel : ViewModelBase
{
    private User _authenticatedUser;
    private IDataService _dataService;
    public ObservableCollection<string> Countries { get; }

    private string _userName = string.Empty;
    private string _password = string.Empty;
    private string _passwordConfirmation = string.Empty;
    private string _country = string.Empty;
    private Visibility _editInfoOn = Visibility.Collapsed;
    private bool _canEdit = false;

    public User AuthenticatedUser
    {
        get => _authenticatedUser;
        set
        {
            _authenticatedUser = value;

            OnPropertyChanged();
        }
    }

    public string UserName
    {
        get => _userName;
        set { _userName = value; OnPropertyChanged(); }
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

    public bool CanEdit
    {
        get => _canEdit;
        set
        {
            _canEdit = value;
            if (_canEdit is true)
            {
            }
            OnPropertyChanged();
        }
    }

    public Visibility EditInfoOn
    {
        get => _editInfoOn;
        set
        {
            _editInfoOn = value;
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



    public RelayCommand EnableEditCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    public RelayCommand UpdateUserCommand { get; set; }


    public UserProfileViewModel(IDataService dataService)
    {
        _dataService = dataService;
        _authenticatedUser = _dataService.GetAuthenticatedUser();
        Countries = new ObservableCollection<string>(_dataService.Countries());
        setUserData();
        EnableEditCommand = new RelayCommand(execute: e => enableEditMode(), canExecute: e => true);
        CancelCommand = new RelayCommand(execute: e => disableEditMode(), canExecute: e => true);
        UpdateUserCommand = new RelayCommand(execute: e => saveChanges(), canExecute: e => true);
    }


    private void enableEditMode()
    {
        CanEdit = true;
        EditInfoOn = Visibility.Visible;
    }

    private void disableEditMode()
    {
        CanEdit = false;
        EditInfoOn = Visibility.Collapsed;
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
        _country = _authenticatedUser.Country;
    }

    private void saveChanges()
    {
        User user;
        try
        {

            user = _dataService.GetByUserName(UserName);

            if (user is not null && user.UserName != _authenticatedUser.UserName)
            {
                Errors["UserName"] = new List<string>() { "Username is taken" };
                NotifyErrorsChanged("UserName");
                return;
            }


        }
        catch (UserNotFound ex)
        {
            Errors.Remove("UserName");
            NotifyErrorsChanged("UserName");
            user = _dataService.GetByUserName(_authenticatedUser.UserName);
        }

        if (!string.Equals(Password, PasswordConfirmation))
        {
            Errors["PasswordConfirmation"] = new List<string>() { "Passwords don't match" };
            NotifyErrorsChanged("PasswordConfirmation");
            return;
        }

        else
        {

            if (!string.IsNullOrEmpty(Password))
            {
                _authenticatedUser.Password = Password;
                user.Password = Password;

            }

            if (!string.IsNullOrEmpty(UserName))
            {
                user.UserName = UserName;
                _authenticatedUser.UserName = UserName;
            }


            _authenticatedUser.Country = Country;
            user.Country = Country;

            _authenticatedUser = _dataService.UpdateUser(user);

            Password = string.Empty;
            PasswordConfirmation = string.Empty;

            Errors.Remove("Password");
            NotifyErrorsChanged("Password");

            Errors.Remove("PasswordConfirmation");
            NotifyErrorsChanged("PasswordConfirmation");


            disableEditMode();
        }
    }

    internal void UpdateData()
    {
        _authenticatedUser = _dataService.GetAuthenticatedUser();
        UserName = AuthenticatedUser.UserName;
    }
}
