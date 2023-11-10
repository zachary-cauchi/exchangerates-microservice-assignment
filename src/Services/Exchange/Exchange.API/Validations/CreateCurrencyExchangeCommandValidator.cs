using FluentValidation;

namespace Exchange.API.Validations
{
    public class CreateCurrencyExchangeCommandValidator : AbstractValidator<CreateCurrencyExchangeCommand>
    {
        public CreateCurrencyExchangeCommandValidator(ILogger<CreateCurrencyExchangeCommandValidator> logger)
        {
            //RuleFor(command => command.UserId).NotEmpty();
            RuleFor(command => command.FromAccountBalanceId).NotEmpty();
            RuleFor(command => command.ToAccountBalanceId).NotEmpty();
            RuleFor(command => command.DebitAmount).NotEmpty();
            RuleFor(command => command.TimeOfRate).NotEmpty().Must(BeValidTimeOfRate).WithMessage("Time of rate cannot be newer than the current time.");
            //RuleFor(command => command.FromCurrencyId).NotEmpty();
            //RuleFor(command => command.ToCurrencyId).NotEmpty();
            RuleFor(command => command.ExchangeRate).NotEmpty().GreaterThan(0);
        }

        private bool BeValidTimeOfRate(DateTime dateTime)
        {
            return dateTime <= DateTime.UtcNow;
        }
    }
}
