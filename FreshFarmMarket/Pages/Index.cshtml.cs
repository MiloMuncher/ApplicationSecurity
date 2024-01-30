using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreshFarmMarket.Pages
{
	[Authorize]
	public class IndexModel : PageModel
	{
		private readonly IHttpContextAccessor contxt;
		public IndexModel(IHttpContextAccessor httpContextAccessor)
		{
			contxt = httpContextAccessor;
		}
		public void OnGet()
		{

		}
	}
}