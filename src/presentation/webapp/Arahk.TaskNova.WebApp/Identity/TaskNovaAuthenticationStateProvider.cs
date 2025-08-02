using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Arahk.TaskNova.WebApp.Identity;

public class TaskNovaAuthenticationStateProvider(UserService userService, ProtectedSessionStorage sessionStorage) : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new (new ClaimsIdentity());
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await UserService.GetSessionAsync("");

            if (userSession == null)
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }

            var identity = new ClaimsIdentity([

                new Claim(ClaimTypes.Name, userSession.Email),
                new Claim(ClaimTypes.NameIdentifier, userSession.Email),
                new Claim(ClaimTypes.Role, userSession.Role)
            ], "TaskNova");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationStateAsync(UserAccount? userAccount)
    {
        var claimsPrincipal = default(ClaimsPrincipal);

        if (userAccount != null)
        {
            await UserService.AddSessionAsync(userAccount);

            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity([
                new Claim(ClaimTypes.Name, userAccount.Email),
                new Claim(ClaimTypes.NameIdentifier, userAccount.Name),
                new Claim(ClaimTypes.Role, userAccount.Role)
            ],"TaskNova"));
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
    
    public async Task RemoveAuthenticationStateAsync(string name)
    {
        await UserService.RemoveSessionAsync(name);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}