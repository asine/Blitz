using System;
using System.Text;

using FluentValidation;
using FluentValidation.Results;

namespace Blitz.Client.Trading.Quote.Edit
{
    public static class ModelValidationHelper
    {
        public static ValidationResult Validate<TValidation, TModel>(TModel model)
            where TModel : ModelWithValidation<TModel, TValidation>
            where TValidation : AbstractValidator<TModel>, new()
        {
            IValidator<TModel> validator = new TValidation();
            return validator.Validate(model);
        }

        public static string GetError(ValidationResult result)
        {
            var validationErrors = new StringBuilder();

            foreach (var validationFailure in result.Errors)
            {
                validationErrors.Append(validationFailure.ErrorMessage);
                validationErrors.Append(Environment.NewLine);
            }

            return validationErrors.ToString();
        }
    }
}