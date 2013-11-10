using FluentValidation;

namespace Blitz.Client.Customer.ReportParameters
{
    public class ReportParameterStepValidator : AbstractValidator<ReportParameterStepViewModel>
    {
        public ReportParameterStepValidator()
        {
            RuleFor(x => x.SelectedDate).NotNull();
        }
    }
}