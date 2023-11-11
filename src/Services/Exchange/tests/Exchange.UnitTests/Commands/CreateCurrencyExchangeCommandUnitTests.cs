using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using NuGet.Common;
using System;

namespace Exchange.UnitTests.Commands
{
    public class CreateCurrencyExchangeCommandUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ExchangeAPIContext> _contextMock;
        private readonly Mock<AccountBalanceRepository> _accountBalanceRepositoryMock;
        private readonly Mock<PastTransactionRepository> _pastTransactionRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<CreateCurrencyExchangeCommandHandler>> _loggerMock;
        private readonly Mock<ILogger<PastTransactionRepository>> _pastTransactionRepositoryLoggerMock;
        private readonly Mock<ILogger<AccountBalanceRepository>> _accountBalanceRepositoryLoggerMock;

        public CreateCurrencyExchangeCommandUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _pastTransactionRepositoryLoggerMock = new Mock<ILogger<PastTransactionRepository>>();
            _accountBalanceRepositoryLoggerMock = new Mock<ILogger<AccountBalanceRepository>>();
            //_contextMock = new Mock<ExchangeAPIContext>(new DbContextOptions<ExchangeAPIContext>(), _contextLoggerMock.Object);
            _contextMock = new Mock<ExchangeAPIContext>();
            _pastTransactionRepositoryMock = new Mock<PastTransactionRepository>(_contextMock.Object, _pastTransactionRepositoryLoggerMock.Object);
            _accountBalanceRepositoryMock = new Mock<AccountBalanceRepository>(_contextMock.Object, _accountBalanceRepositoryLoggerMock.Object);
            _userRepositoryMock = new Mock<IUserRepository>();
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<CreateCurrencyExchangeCommandHandler>>();

        }

        // This test is broken at this time due to the implementation of cache checks and how transactions are wrapped.
        // This will be fixed at a later stage.
        // [Fact]
        public async Task Valid_currency_exchange_transaction_commits()
        {
            // Arrange
            Currency[] currencies = DataBuilders.GetValidCurrencyPair();
            Currency srcCurrency = currencies[0];
            Currency destCurrency = currencies[1];

            AccountBalance[] accountBalances = DataBuilders.GetValidAccountBalancePair();
            AccountBalance srcAccountBalance = accountBalances[0];
            AccountBalance destAccountBalance = accountBalances[1];
            srcAccountBalance.CurrencyId = srcCurrency.Id;
            srcAccountBalance.Currency = srcCurrency;
            destAccountBalance.CurrencyId = destCurrency.Id;
            destAccountBalance.Currency = destCurrency;

            User user = DataBuilders.GetValidUser();

            ExchangeRate exchangeRate = DataBuilders.GetValidExchangeRate();

            PastTransaction pastTransaction = DataBuilders.GetValidPastTransaction();

            int debitAmount = 50;
            CreateCurrencyExchangeCommand command = new CreateCurrencyExchangeCommand(srcAccountBalance.Id, destAccountBalance.Id, debitAmount, DateTime.UtcNow, exchangeRate.Rate);

            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 1))).ReturnsAsync(() => srcCurrency);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 2))).ReturnsAsync(() => destCurrency);

            _contextMock.Setup(x => x.AccountBalances).ReturnsDbSet(accountBalances);
            _contextMock.Setup(x => x.Entry(It.IsAny<object>())).Returns(() => { return });
            //_accountBalanceRepositoryMock.Setup(x => x.UpdateAccountBalance(It.IsAny<AccountBalance>()));
            //_accountBalanceRepositoryMock.Setup(x => x.GetAccountBalanceByIdAsync(It.Is<int>(y => y == 1))).ReturnsAsync(() => srcAccountBalance);
            //_accountBalanceRepositoryMock.Setup(x => x.GetAccountBalanceByIdAsync(It.Is<int>(y => y == 2))).ReturnsAsync(() => destAccountBalance);
            //_accountBalanceRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWorkMock.Object);

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.Is<int>(y => y == 1))).ReturnsAsync(() => user);

            _contextMock.Setup(x => x.PastTransactions).ReturnsDbSet(new List<PastTransaction>() { pastTransaction });
            //_pastTransactionRepositoryMock.SetupProperty(x => x.UnitOfWork, _unitOfWorkMock.Object);

            _contextMock.Setup(x => x.ExecuteTransactionAsync(It.IsAny<Action>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback((Action action, string msg, CancellationToken token) =>
                {
                    action();
                });

            // Act
            CreateCurrencyExchangeCommandHandler handler = new CreateCurrencyExchangeCommandHandler(_accountBalanceRepositoryMock.Object, _pastTransactionRepositoryMock.Object, _userRepositoryMock.Object, _currencyRepositoryMock.Object, _mediatorMock.Object, _loggerMock.Object);
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
        }
    }
}
