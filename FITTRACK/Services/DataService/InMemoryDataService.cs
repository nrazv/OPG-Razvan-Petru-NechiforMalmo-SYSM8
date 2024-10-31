using FITTRACK.Models;
using FITTRACK.Data;
using FITTRACK.AppExceptions;
using System.Linq;
using System;
using FITTRACK.Services.Navigation;
using System.Collections.ObjectModel;

namespace FITTRACK.Services.DataService;

public class InMemoryDataService : IDataService
{
    private INavigationService _navigationService;
    private DataContext _context;
    private User _authenticatedUser;
    public User AuthenticatedUser { get => _authenticatedUser; set => _authenticatedUser = value; }

    public InMemoryDataService(DataContext context, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _context = context;
    }

    public bool AddUser(User person)
    {
        var userExists = _context.Users.Where(u => u.Value.UserName == person.UserName);



        if (userExists.Any())
        {
            throw new UserTaken();
        }
        else
        {
            _context.Users.Add(person.Id, person);
        }

        return true;
    }

    public List<string> Countries()
    {
        return _context.CountriesList;
    }

    public User GetByUserName(string username)
    {
        var user = _context.Users.Where(u => u.Value.UserName == username);

        if (!user.Any())
        {
            throw new UserNotFound();
        }

        return (User)user.First().Value;
    }

    public List<SecurityQuestion> GetSecurityQuestions() => _context.SecurityQuestions;

    public User? Login(UserCredentials credentials)
    {
        var userToLogin = GetByUserName(credentials.UserName);
        var passwordMatch = userToLogin.Password.Equals(credentials.Password);
        if (!passwordMatch)
        {
            throw new InvalidCredentials();
        }

        User? user = userToLogin;


        AuthenticatedUser = userToLogin;
        return user;
    }

    public void LogOut()
    {
        AuthenticatedUser = null;
    }

    public User UpdateUser(User user)
    {


        var userToUpdate = (User)_context.Users.Where(u => u.Value.Id == user.Id).First().Value;
        userToUpdate.UserName = user.UserName;
        userToUpdate.Password = user.Password;
        userToUpdate.Country = user.Country;


        _context.Users[user.Id] = userToUpdate;

        return (User)_context.Users.Where(u => u.Value.Id == user.Id).First().Value;
    }

    public List<User> AllUsers()
    {
        List<User> users = new List<User>();

        foreach (var item in _context.Users.AsEnumerable())
        {
            users.Add(item.Value as User);
        }

        return users;
    }

    public void DeleteUserWorkout(Workout workout, string userName)
    {
        _context.Users.Where(u => u.Value.UserName == userName).First().Value.DeleteWorkout(workout);
    }

}

