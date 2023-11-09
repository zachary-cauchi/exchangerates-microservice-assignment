namespace Exchange.API.Models
{
    public class User : IAggregateRoot
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = string.Empty;

        public ICollection<AccountBalance> Accounts { get; } = new List<AccountBalance>();
    }
}
