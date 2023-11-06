namespace WeatherAPI.Models.Responses;

public class LoginResponse
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Token { get; set; }
    public string? Email { get; set; }

    public LoginResponse(string token, string email, string name, string surname)
    {
        Token = token;
        Email = email;
        Name = name;
        Surname = surname;
    }
}
