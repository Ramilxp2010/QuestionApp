using Microsoft.AspNetCore.Components.Authorization;

namespace QuestionAppUI.Helpers
{
    public static class AuthenticationStateProviderHelper
    {
        public static async Task<User> GetUserFromAuth(this AuthenticationStateProvider provider, IUserData userData) 
        {
            var authState = await provider.GetAuthenticationStateAsync();
            string objectId = authState.User.Claims.FirstOrDefault(x => x.Type.Contains("ObjectIdentifier"))?.Value;
            if (string.IsNullOrWhiteSpace(objectId))
            {
                return null;
            }

            return await userData.GetUserByIdentifier(objectId);
        }
    }
}
