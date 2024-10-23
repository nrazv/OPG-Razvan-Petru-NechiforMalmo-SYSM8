using FITTRACK.Models;
using System.Xml.Linq;


namespace FITTRACK.Data;

public class DataContext
{
    private readonly Dictionary<string, Person> _userDataContext = new();
    private readonly List<SecurityQuestion> _securityQuestions = new();
    private List<string> _conutriesList;
    public List<string> CountriesList { get => _conutriesList; }
    public List<SecurityQuestion> SecurityQuestions { get => _securityQuestions; }
    public Dictionary<string, Person> Users { get => _userDataContext; }

    public DataContext()
    {
        _createAdmin();
        _createSecurityQescions();
        _addCountries();
        createDefaultUser();
    }


    private void _createSecurityQescions()
    {
        _securityQuestions.Add(new SecurityQuestion(1, "What was the name of your first pet?"));
        _securityQuestions.Add(new SecurityQuestion(2, "In what city were you born?"));
        _securityQuestions.Add(new SecurityQuestion(2, "What was the model of your first car?"));
        _securityQuestions.Add(new SecurityQuestion(4, "What is your mother’s maiden name?"));
        _securityQuestions.Add(new SecurityQuestion(5, "What was the name of your first school?"));
    }

    private void createDefaultUser()
    {
        User user = new User();
        user.UserName = "razvan";
        user.Password = "password22!";
        user.Country = "Romania";
        user.SecurityQuestion = new UserSecurityQuestion(_securityQuestions[0], "Avrig");
        Users.Add(user.UserName, user);
    }

    private void _createAdmin()
    {
        var admin = new Admin() { UserName = "admin", Password = "password" };
        _userDataContext.Add(admin.UserName, admin);
    }


    private void _addCountries()
    {
        _conutriesList = new List<string>()
        {
            "Afghanistan", "Albania", "Algeria", "Andorra",
            "Angola", "Antigua & Deps", "Argentina", "Armenia",
            "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain",
            "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize",
            "Benin", "Bhutan", "Bolivia", "Bosnia Herzegovina", "Botswana",
            "Brazil", "Brunei", "Bulgaria", "Burkina", "Burundi", "Cambodia",
            "Cameroon", "Canada", "Cape Verde", "Central African Rep", "Chad",
            "Chile", "China", "Colombia", "Comoros", "Congo", "Congo Democratic Rep",
            "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark",
            "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt",
            "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Fiji", "Finland",
            "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea",
            "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq",
            "Ireland Republic", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan",
            "Kazakhstan", "Kenya", "Kiribati", "Korea North", "Korea South", "Kosovo", "Kuwait",
            "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya",
            "Liechtenstein", "Lithuania", "Luxembourg", "Macedonia", "Madagascar", "Malawi",
            "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius",
            "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique",
            "Myanmar, Burma", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger",
            "Nigeria", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru",
            "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russian Federation", "Rwanda",
            "St Kitts & Nevis", "St Lucia", "Saint Vincent & the Grenadines", "Samoa", "San Marino",
            "Sao Tome & Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone",
            "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Sudan",
            "Spain", "Sri Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan",
            "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey",
            "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates",
            "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu",
            "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe"
        };
    }

}