using FluentValidation;

namespace Blitz.Client.CRM.Client.Edit
{
    public class ClientValidator : AbstractValidator<ClientModel>
    {
        public ClientValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty().NotNull();
        }
    }
}