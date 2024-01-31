using FreshFarmMarket.Model;
using FreshFarmMarket.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

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
				LModel.RememberMe, lockoutOnFailure: true);
				if (identityResult.Succeeded)
				{
                    contxt.HttpContext.Session.Clear();
                    contxt.HttpContext.Session.CommitAsync();

                    var user = await userManager.FindByEmailAsync(LModel.Email);
					if (user != null)
					{
						await userManager.ResetAccessFailedCountAsync(user);

						contxt.HttpContext.Session.SetString("Full Name", user.FullName);
						contxt.HttpContext.Session.SetString("Credit Card Number", protector.Unprotect(user.CreditCard));
						contxt.HttpContext.Session.SetString("Gender", user.Gender);
						contxt.HttpContext.Session.SetString("Mobile Number", user.PhoneNumber);
						contxt.HttpContext.Session.SetString("Delivery Address", user.DeliveryAddress);
						contxt.HttpContext.Session.SetString("Email", user.Email);
						contxt.HttpContext.Session.SetString("About Me", user.AboutMe);

                        var authCookieValue = Guid.NewGuid().ToString(); // Generate a unique value
                        contxt.HttpContext.Response.Cookies.Append("AuthCookie", authCookieValue);
                        contxt.HttpContext.Session.SetString("AuthCookie", authCookieValue);
						user.AuthToken = authCookieValue;
						await userManager.UpdateAsync(user);


                        return RedirectToPage("Index");
					}
				}
				ModelState.AddModelError("", "Username or Password incorrect");
                if (identityResult.IsLockedOut)
                {
                    // Account is locked out
                    ModelState.AddModelError("", "Account is locked out due to multiple failed login attempts. Please try again later.");
                }
                else
                {
                    // Failed login attempt
                    ModelState.AddModelError("", "Username or Password incorrect");
                }
            }
			return Page();
		}
		public class Response
		{
			public bool Success {  get; set; }
		}
		public bool ValidateCaptcha()
		{
			string Response = Request.Form["g-recaptcha-response"];
			bool valid = false;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create
			("https://www.google.com/recaptcha/api/siteverify?secret=6LeW32ApAAAAAHhya0qWyDxkE611cGqXa1_OlKZN &response=" + Response);
			try
			{
				using (WebResponse wResponse = request.GetResponse())
				{
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();

						var data = JsonSerializer.Deserialize<Response>(jsonResponse);

						valid = Convert.ToBoolean(data.Success);
					}
				}
				return valid;
			}
			catch (WebException ex)
			{
				throw ex;
			}

		}
	}
}
