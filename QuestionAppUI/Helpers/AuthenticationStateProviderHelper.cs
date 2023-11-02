using Microsoft.AspNetCore.Components.Authorization;

namespace QuestionAppUI.Helpers
{
    public static class AuthenticationStateProviderHelper
    {
        public static async Task<UserModel> GetUserFromAuth(this AuthenticationStateProvider provider, IUserData userData) 
        {
            var authState = await provider.GetAuthenticationStateAsync();
            string objectId = authState.User.Claims.FirstOrDefault(x => x.Type.Contains("ObjectIdentifier"))?.Value;
            return await userData.GetUserByIdentifier(objectId);
        }
    }
}
