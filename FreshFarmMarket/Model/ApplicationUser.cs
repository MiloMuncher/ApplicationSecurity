using Microsoft.AspNetCore.Identity;

namespace FreshFarmMarket.Model
{
	public class ApplicationUser : IdentityUser
	{
		
		public string FullName {  get; set; }
		public string CreditCard { get; set; }
		public int MobileNumber {  get; set; }
		public string Gender { get; set; }
		public string DeliveryAddress {  get; set; }
		public string AboutMe {  get; set; }
	}
}
