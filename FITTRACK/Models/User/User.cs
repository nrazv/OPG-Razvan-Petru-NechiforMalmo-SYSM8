
namespace FITTRACK.Models;
public class User : Person
{
    public string Country { get; set; }
    public UserSecurityQuestion SecurityQuestion { get; set; }
}
