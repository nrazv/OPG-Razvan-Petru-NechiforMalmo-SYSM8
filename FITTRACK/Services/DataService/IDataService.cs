using FITTRACK.Models;
using System.Collections.ObjectModel;

namespace FITTRACK.Services.DataService;

public interface IDataService
{
    public bool AddUser(User user);
    public User? Login(UserCredentials credentials);
    public void LogOut();
    public User GetByUserName(string userName);
    public List<SecurityQuestion> GetSecurityQuestions();
    public List<string> Countries();
    public User UpdateUser(User user);
    public List<User> AllUsers();
    public User GetAuthenticatedUser();
    public void DeleteUserWorkout(Guid workoutId, Guid userId);

}
