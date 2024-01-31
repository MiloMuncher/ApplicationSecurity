using FreshFarmMarket.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WebApp_Core_Identity.Model;

namespace FreshFarmMarket.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;

		private readonly IHttpContextAccessor contxt;
        private readonly AuthDbContext authDbContext;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, AuthDbContext authDbContext, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.authDbContext = authDbContext;
			this.userManager = userManager;
            contxt = httpContextAccessor;
        }
        public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			var user = await userManager.GetUserAsync(User);

			contxt.HttpContext.Session.Remove("AuthCookie");
            contxt.HttpContext.Session.Clear();
            contxt.HttpContext.Session.CommitAsync();

            // Expire AuthCookie
            contxt.HttpContext.Response.Cookies.Delete("AuthCookie");

			var log = new AuditLog
			{
				UserId = user.Id,
				LogInOrOut = "Logged Out",
				Time = DateTime.Now,
			};

			authDbContext.AuditLogTable.Add(log);
			authDbContext.SaveChanges();

			await signInManager.SignOutAsync();
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
