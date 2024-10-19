

namespace FITTRACK.Models;

public abstract class Person
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public abstract void SignIn();
}
