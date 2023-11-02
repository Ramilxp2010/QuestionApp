using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace QuestionAppUI.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private ClaimsPrincipal _anonymos = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage) 
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSessionStorage = await _sessionStorage.GetAsync<UserSession>(nameof(UserSession));
                var userSession = userSessionStorage.Success ? userSessionStorage.Value : null;

                if (userSession == null)
                    return await Task.FromResult(new AuthenticationState(_anonymos));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Role, userSession.Role),
                    new Claim("ObjectIdentifier", userSession.Identifier)
                }, "CustomAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymos));
            }
        }

        public async Task UpdateAuthenticationState(UserSession userSession) 
        {
            ClaimsPrincipal claimsPrincipal;
            if (userSession != null) 
            {
                await _sessionStorage.SetAsync(nameof(UserSession), userSession);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Role, userSession.Role),
                    new Claim("ObjectIdentifier", userSession.Identifier)
                }));
            }
            else
            {
                await _sessionStorage.DeleteAsync(nameof(UserSession));
                claimsPrincipal = _anonymos;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
