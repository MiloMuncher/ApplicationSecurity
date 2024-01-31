using FreshFarmMarket.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor contxt;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            contxt = httpContextAccessor;
        }
        public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostLogoutAsync()
		{
            contxt.HttpContext.Session.Remove("AuthCookie");
            contxt.HttpContext.Session.Clear();
            contxt.HttpContext.Session.CommitAsync();

            // Expire AuthCookie
            contxt.HttpContext.Response.Cookies.Delete("AuthCookie");

   
            await signInManager.SignOutAsync();
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
