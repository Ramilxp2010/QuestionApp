using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
        public LoginModel(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = QuestionAppUI.Data.SampleData.AuthUsers.FirstOrDefault(u => u.UserName == UserVM.UserName && u.Password == UserVM.Password);
            if (user == null)
                return Page();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("ObjectIdentifier", user.Identifier)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            try
            {
                await _accessor.HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity),
                   new AuthenticationProperties());

                return Redirect("/");
            }
            catch 
            {
                return Page();
            }

        }
    }
}
