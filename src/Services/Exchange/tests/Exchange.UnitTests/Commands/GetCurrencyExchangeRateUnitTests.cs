using System.Text;

namespace Exchange.UnitTests.Commands
{
    public class GetCurrencyExchangeRateUnitTests
    {
        private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
        private readonly Mock<IDistributedCache> _cacheMock; 
        private readonly Mock<IExchangeRateService> _exchangeRateServiceMock;
        private readonly Mock<ILogger<GetCurrencyExchangeRateCommandHandler>> _loggerMock;
        private readonly Mock<IUnitOfWork> _contextMock;

        public GetCurrencyExchangeRateUnitTests()
        {
            _currencyRepositoryMock = new Mock<ICurrencyRepository>();
            _cacheMock = new Mock<IDistributedCache>();
            _exchangeRateServiceMock = new Mock<IExchangeRateService>();
            _loggerMock = new Mock<ILogger<GetCurrencyExchangeRateCommandHandler>>();
            _contextMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Valid_currency_exchange_in_cache_returned()
        {
            // Arrange
            Currency srcCurrency = new Currency() { Id = 1, Code = "EUR" };
            Currency destCurrency = new Currency() { Id = 2, Code = "USD" };
            
            ExchangeRate exchangeRate = new ExchangeRate(srcCurrency.Code, destCurrency.Code, DateTime.UtcNow, "Test", 2);
            string exchangeRateSerialized = JsonSerializer.Serialize(exchangeRate);
            byte[] exchangeRateBytes = Encoding.UTF8.GetBytes(exchangeRateSerialized);

            GetCurrencyExchangeRateCommand command = new GetCurrencyExchangeRateCommand(1, 2);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 1))).ReturnsAsync(() => srcCurrency);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 2))).ReturnsAsync(() => destCurrency);

            // Extension methods aren't supported, so we have to mock the internal non-extended function.
            _cacheMock.Setup(x => x.GetAsync(It.Is<string>(y => y == exchangeRate.Key), It.IsAny<CancellationToken>())).ReturnsAsync(() => exchangeRateBytes);

            // Act
            GetCurrencyExchangeRateCommandHandler handler = new GetCurrencyExchangeRateCommandHandler(_currencyRepositoryMock.Object, _cacheMock.Object, _loggerMock.Object, _exchangeRateServiceMock.Object);
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ExchangeRate>(result);
            Assert.Equal(exchangeRateSerialized, JsonSerializer.Serialize(result));
        }
    }
}