using FluentValidation;

namespace Blitz.Client.CRM.Client.Edit
{
    public class ClientValidation : AbstractValidator<ClientModel>
    {
        public ClientValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty().NotNull();
        }
    }
}