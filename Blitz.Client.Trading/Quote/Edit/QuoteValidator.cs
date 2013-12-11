using FluentValidation;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteValidator : AbstractValidator<QuoteModel>
    {
        public QuoteValidator()
        {
            RuleFor(x => x.Instrument).NotNull();
            RuleFor(x => x.Notes).NotEmpty();
        }
    }
}