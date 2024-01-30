using FreshFarmMarket.Model;
using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
	[ValidateAntiForgeryToken]
	public class LoginModel : PageModel
    {
		[BindProperty]
		public Login LModel { get; set; }

		private readonly IHttpContextAccessor contxt;
		private UserManager<ApplicationUser> userManager { get; }

		private readonly SignInManager<ApplicationUser> signInManager;
		public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			contxt = httpContextAccessor;
		}


		public void OnGet()
        {

        }
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
				LModel.RememberMe, false);
				if (identityResult.Succeeded)
				{
					var user = await userManager.FindByEmailAsync(LModel.Email);
					contxt.HttpContext.Session.SetString("Full Name", user.FullName);
					contxt.HttpContext.Session.SetString("Credit Card Number", protector.Unprotect(user.CreditCard));
					contxt.HttpContext.Session.SetString("Gender", user.Gender);
					contxt.HttpContext.Session.SetString("Mobile Number", user.PhoneNumber);
					contxt.HttpContext.Session.SetString("Delivery Address", user.DeliveryAddress);
					contxt.HttpContext.Session.SetString("Email", user.Email);
					contxt.HttpContext.Session.SetString("About Me", user.AboutMe);


					return RedirectToPage("Index");
				}
				ModelState.AddModelError("", "Username or Password incorrect");
			}
			return Page();
		}
	}
}
