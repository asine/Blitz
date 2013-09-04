using FluentValidation;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteValidation : AbstractValidator<QuoteModel>
    {
        public QuoteValidation()
        {
            RuleFor(x => x.Instrument).NotNull();
            RuleFor(x => x.Notes).NotEmpty();
        }
    }
}