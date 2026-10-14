using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestionAppUI.Models;
using System.Security.Claims;

namespace QuestionAppUI.Areas.Identity.Account.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public AuthUserModel UserVM { get; set; }

        private readonly IHttpContextAccessor _accessor;
        private readonly IUserData _userData;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginModel(
            IHttpContextAccessor accessor,
            IUserData userData,
            IPasswordHasher<User> passwordHasher)
        {
            _accessor = accessor;
            _userData = userData;
            _passwordHasher = passwordHasher;
        }

        public IActionResult OnGet()
        {
            if (_accessor.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return Redirect("/");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userData.GetUserByUserName(UserVM.UserName);
            if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, UserVM.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("ObjectIdentifier", user.ObjectIdentifier)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _accessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties());

            return Redirect("/");
        }
    }
}
