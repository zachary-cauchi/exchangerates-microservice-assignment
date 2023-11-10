namespace Exchange.API.Data
{
    public class ExchangeAPIContext : DbContext, IUnitOfWork
    {
        private readonly ILogger<ExchangeAPIContext> _logger;

        public DbSet<Currency> Currency { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AccountBalance> AccountBalances { get; set; }

        public DbSet<PastTransaction> PastTransactions { get; set; }

        private IDbContextTransaction? _currentTransaction = null;

        public IDbContextTransaction? CurrentTransaction { get; }

        public bool HasActiveTransaction => _currentTransaction != null;

        public ExchangeAPIContext(DbContextOptions<ExchangeAPIContext> options, ILogger<ExchangeAPIContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CurrencyEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new AccountBalanceEntityTypeConfiguration());
            builder.ApplyConfiguration(new PastTransactionEntityTypeConfiguration());
        }

        public Task ExecuteTransactionAsync(Action transactionChanges, string message, CancellationToken? cancellationToken = null)
        {
            var strategy = Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await BeginTransactionAsync();
                using (_logger.BeginScope($"ExchangeAPIContext::{message}"))
                {
                    _logger.LogInformation("Begin transaction {TransactionId} ({message}).", transaction.TransactionId, message);

                    try
                    {
                        transactionChanges();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Could not complete transaction {transaction.TransactionId} ({message}).", ex);

                        return;
                    }

                    _logger.LogInformation("Commit transaction {TransactionId} ({message}).", transaction.TransactionId, message);

                    await CommitTransactionAsync(transaction);

                    _logger.LogInformation("Completed transaction {TransactionId} ({message}).", transaction.TransactionId, message);
                }
            });
        }

        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction of id {transaction.TransactionId} does not match the current transaction.");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                DisposeTransaction();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                DisposeTransaction();
            }
        }

        private void DisposeTransaction()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}
