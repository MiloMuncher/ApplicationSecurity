using FreshFarmMarket.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using WebApp_Core_Identity.Model;

namespace FreshFarmMarket.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor contxt;

        private readonly AuthDbContext authContext;
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthDbContext authContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            contxt = httpContextAccessor;
            this.authContext = authContext;
        }

        public async Task<IActionResult> OnGet()
        {
            var authCookieValue = contxt.HttpContext.Request.Cookies["AuthCookie"];
            var sessionAuthToken = contxt.HttpContext.Session.GetString("AuthCookie");
            var user = await userManager.GetUserAsync(User);

            if (sessionAuthToken != user.AuthToken)
            {
                user.AuthToken = null;
                await userManager.UpdateAsync(user);

                await signInManager.SignOutAsync();
                return RedirectToPage("Login");
            }
            var log = new AuditLog
            {
                UserId = user.Id,
                LogInOrOut = "Logged In",
                Time = DateTime.Now,
            };

            authContext.AuditLogTable.Add(log);
            authContext.SaveChanges();
            return Page();
        }
    }
}
