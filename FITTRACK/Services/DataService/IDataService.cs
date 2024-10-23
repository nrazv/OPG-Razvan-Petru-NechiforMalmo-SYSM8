using FITTRACK.Models;

namespace FITTRACK.Services.DataService;

public interface IDataService
{
    public bool AddUser(Person person);
    public User? Login(UserCredentials credentials);
    public User GetByUserName(string userName);
    public List<SecurityQuestion> GetSecurityQuestions();
    public List<string> Countries();

}
