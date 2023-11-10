namespace Exchange.API.Models
{
    public class User : IAggregateRoot
    {
        public int Id { get; private set; }

        public string? FirstName { get; private set; }

        public string? LastName { get; private set; }

        public string Email { get; private set; } = string.Empty;

        private readonly List<AccountBalance> _accountBalances = new List<AccountBalance>();
        public IReadOnlyCollection<AccountBalance> AccountBalances => _accountBalances;

        public User()
        {
        }

        public User(int id, string email, string? firstName, string? lastName, List<AccountBalance>? accountBalances = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            if (accountBalances != null)
            {
                _accountBalances.AddRange(accountBalances);
            }
        }

        public void AddAccountBalance(AccountBalance accountBalance)
        {
            if (accountBalance == null)
            {
                throw new ArgumentNullException("Expected a non-null account balance, got null.");
            }

            _accountBalances.Add(accountBalance);
        }

        public void RemoveAccountBalanceById(int id)
        {
            var accountBalance = _accountBalances.First(a => a.Id == id);

            if (accountBalance == null)
            {
                throw new Exception($"No account balance was found with id {id}.");
            }

            _accountBalances.Remove(accountBalance);
        }
    }
}
