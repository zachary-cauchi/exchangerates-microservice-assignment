using Exchange.API.Models;
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
        public async Task Valid_get_exchange_rate_in_cache_returned()
        {
            // Arrange
            Currency[] currencies = DataBuilders.GetValidCurrencyPair();
            Currency srcCurrency = currencies[0];
            Currency destCurrency = currencies[1];

            ExchangeRate exchangeRate = DataBuilders.GetValidExchangeRate();
            string exchangeRateSerialized = JsonSerializer.Serialize(exchangeRate);
            byte[] exchangeRateBytes = Encoding.UTF8.GetBytes(exchangeRateSerialized);

            GetCurrencyExchangeRateCommand command = new GetCurrencyExchangeRateCommand(1, 2);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 1))).ReturnsAsync(() => srcCurrency);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 2))).ReturnsAsync(() => destCurrency);

            DataBuilders.MockCacheKeyForReturnValue(_cacheMock, exchangeRate.Key, exchangeRateSerialized);

            // Act
            GetCurrencyExchangeRateCommandHandler handler = new GetCurrencyExchangeRateCommandHandler(_currencyRepositoryMock.Object, _cacheMock.Object, _loggerMock.Object, _exchangeRateServiceMock.Object);
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ExchangeRate>(result);
            Assert.Equal(exchangeRateSerialized, JsonSerializer.Serialize(result));
        }

        [Fact]
        public async Task Valid_get_exchange_rate_no_cache_api_called_returned()
        {
            // Arrange
            Currency[] currencies = DataBuilders.GetValidCurrencyPair();
            Currency srcCurrency = currencies[0];
            Currency destCurrency = currencies[1];

            ExchangeRate exchangeRate = DataBuilders.GetValidExchangeRate();
            string exchangeRateSerialized = JsonSerializer.Serialize(exchangeRate);
            byte[] exchangeRateBytes = Encoding.UTF8.GetBytes(exchangeRateSerialized);

            GetCurrencyExchangeRateCommand command = new GetCurrencyExchangeRateCommand(1, 2);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 1))).ReturnsAsync(() => srcCurrency);
            _currencyRepositoryMock.Setup(x => x.GetCurrencyByIdAsync(It.Is<int>(y => y == 2))).ReturnsAsync(() => destCurrency);

            _cacheMock.Setup(x => x.GetAsync(It.Is<string>(y => y == exchangeRate.Key), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            _exchangeRateServiceMock.Setup(x => x.GetRateAsync(It.Is<string>(x => x == srcCurrency.Code), It.Is<string>(x => x == destCurrency.Code))).ReturnsAsync(() => exchangeRate).Verifiable();

            // Act
            GetCurrencyExchangeRateCommandHandler handler = new GetCurrencyExchangeRateCommandHandler(_currencyRepositoryMock.Object, _cacheMock.Object, _loggerMock.Object, _exchangeRateServiceMock.Object);
            var result = await handler.Handle(command, default);

            // Assert
            _exchangeRateServiceMock.Verify();
            Assert.NotNull(result);
            Assert.IsType<ExchangeRate>(result);
            Assert.Equal(exchangeRateSerialized, JsonSerializer.Serialize(result));
        }
    }
}