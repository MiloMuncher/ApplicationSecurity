using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FreshFarmMarket.ViewModels
{
	public class Register
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{12,}$", 
			ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
		[DataType(DataType.Password)]
		
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required.")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Full Name is required.")]
		[StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Credit Card Number is required.")]
		[RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid Credit Card Number.")]
		public string CreditCardNumber { get; set; }

		[Required(ErrorMessage = "Gender is required.")]
		public string Gender { get; set; }

		[Required(ErrorMessage = "Mobile Number is required.")]
		[RegularExpression(@"^[8-9][0-9]{7}", ErrorMessage = "Invalid Mobile Number.")]
		public string MobileNumber { get; set; }

		[Required(ErrorMessage = "Delivery Address is required.")]
		public string DeliveryAddress { get; set; }

		[Required(ErrorMessage = "Image is required.")]
		[RegularExpression(@"^[a-zA-Z0-9\s.,'()-]*$", ErrorMessage = "Special characters are not allowed.")]
		public string AboutMe { get; set; }

		}
	}
