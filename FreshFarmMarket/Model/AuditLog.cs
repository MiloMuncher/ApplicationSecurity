namespace FreshFarmMarket.Model
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string LogInOrOut { get; set; }
		public DateTime Time { get; set; }
	}
}
