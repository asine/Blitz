using FluentValidation;

namespace Blitz.Client.Trading.QuoteEdit
{
    public class QuoteValidation : AbstractValidator<QuoteModel>
    {
        public QuoteValidation()
        {
            RuleFor(customer => customer.Instrument).NotNull();
        }
    }
}