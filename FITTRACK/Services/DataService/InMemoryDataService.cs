using FITTRACK.Models;
using FITTRACK.Data;
using FITTRACK.AppExceptions;
using System.Linq;

namespace FITTRACK.Services.DataService;

public class InMemoryDataService : IDataService
{
    private readonly DataContext _context;

    public InMemoryDataService(DataContext context)
    {
        _context = context;
    }

    public bool AddUser(Person person)
    {
        var userExists = _context.Users.ContainsKey(person.UserName);

        if (userExists)
        {
            throw new UserTaken();
        }
        else
        {
            _context.Users.Add(person.UserName, person);
        }

        return true;
    }

    public List<string> Countries()
    {
        return _context.CountriesList;
    }

    public User GetByUserName(string username)
    {
        _context.Users.TryGetValue(username, out var user);

        if (user is null)
        {
            throw new UserNotFound();
        }

        return (User)user;
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
        return user;
    }
}

