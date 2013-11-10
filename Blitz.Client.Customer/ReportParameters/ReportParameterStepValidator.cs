using System;

using FluentValidation;

namespace Blitz.Client.Customer.ReportParameters
{
    public class ReportParameterStepValidator : AbstractValidator<ReportParameterStepViewModel>
    {
        public ReportParameterStepValidator()
        {
            RuleFor(x => x.SelectedDate).Must(x => x > DateTime.Now.AddDays(-2)).WithMessage("xxxxx");
        }
    }
}