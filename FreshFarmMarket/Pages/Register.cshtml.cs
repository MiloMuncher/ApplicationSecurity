using FreshFarmMarket.Model;
using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
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
				var user = new ApplicationUser()
				{
					UserName = RModel.Email,
					Email = RModel.Email,
					FullName = RModel.FullName,
					CreditCard = RModel.CreditCardNumber,
					PhoneNumber = RModel.MobileNumber,
					Gender = RModel.Gender,
					DeliveryAddress = RModel.DeliveryAddress,
					AboutMe = RModel.AboutMe,

				};
				var result = await userManager.CreateAsync(user, RModel.Password);
				if (result.Succeeded)
				{
					await signInManager.SignInAsync(user, false);
					return RedirectToPage("Index");
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
