using Microsoft.AspNetCore.Identity;

namespace FreshFarmMarket.Model
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName {  get; set; }
		public string CreditCard { get; set; }
		public string Gender { get; set; }
		public string DeliveryAddress {  get; set; }
		public string? PhotoURL { get; set; }
		public string AboutMe {  get; set; }
	}
}
