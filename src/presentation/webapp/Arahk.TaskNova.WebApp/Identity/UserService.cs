namespace Arahk.TaskNova.WebApp.Identity;

public class UserAccount
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ConnectionId { get; set; }
}

public class UserService
{
    private static List<UserAccount> _userSessionStorage = [];
    
    private readonly List<UserAccount> _userAccounts =
    [
        new UserAccount { Email = "arahk@outlook.com", Name = "Arahk Yambupah", Password = "Password!123", Role = "Admin" }
    ];

    public static Task AddSessionAsync(UserAccount userAccount)
    {
        _userSessionStorage.Add(userAccount);
        
        return Task.CompletedTask;
    }

    public static Task<UserAccount?> GetSessionAsync(string email)
    {
        return Task.FromResult(_userSessionStorage.FirstOrDefault());
    }

    public static Task RemoveSessionAsync(string email)
    {
        _userSessionStorage.RemoveAll(x => x.Email == email);
        
        return Task.CompletedTask;
    }
    
    public UserAccount? GetByEmail(string email)
    {
        return _userAccounts.FirstOrDefault(x => x.Email == email);   
    }
}