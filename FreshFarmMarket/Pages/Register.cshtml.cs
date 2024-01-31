using FreshFarmMarket.Model;
using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace FreshFarmMarket.Pages
{
	[ValidateAntiForgeryToken]
	public class RegisterModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }
		[BindProperty]

		public Register RModel { get; set; }
		public RegisterModel(UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
				var protector = dataProtectionProvider.CreateProtector("MySecretKey");

				var user = new ApplicationUser()
				{
					UserName = HttpUtility.HtmlEncode(RModel.Email),
					Email = HttpUtility.HtmlEncode(RModel.Email),
					FullName = HttpUtility.HtmlEncode(RModel.FullName),
					CreditCard = protector.Protect(RModel.CreditCardNumber),
					PhoneNumber = HttpUtility.HtmlEncode(RModel.MobileNumber),
					Gender = HttpUtility.HtmlEncode(RModel.Gender),
					DeliveryAddress = HttpUtility.HtmlEncode(RModel.DeliveryAddress),
					AboutMe = HttpUtility.HtmlEncode(RModel.AboutMe),
					ProfileImage = RModel.ProfileImage,


				};
				
                var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{

					return RedirectToPage("Login");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

			}
			return Page();
		}
		public void OnGet()
        {
        }
    }

}
