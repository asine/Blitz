using System;
using System.ComponentModel;
using System.Linq;

using FluentValidation;
using FluentValidation.Results;

using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Quote.Edit
{
    public abstract class ModelWithValidation<TModel, TValidation> : NotifyPropertyChanged, IDataErrorInfo
        where TModel : ModelWithValidation<TModel, TValidation>
        where TValidation : AbstractValidator<TModel>, new()
    {
        public ValidationResult SelfValidate()
        {
            return ModelValidationHelper.Validate<TValidation, TModel>((TModel) this);
        }

        #region IDataErrorInfo Members

        public string Error
        {
            get { return ModelValidationHelper.GetError(SelfValidate()); }
        }

        public string this[string columnName]
        {
            get
            {
                var validationResults = SelfValidate();
                if (validationResults == null) return string.Empty;
                var columnResults = validationResults.Errors.FirstOrDefault(x => string.Compare(x.PropertyName, columnName, StringComparison.OrdinalIgnoreCase) == 0);
                return columnResults != null ? columnResults.ErrorMessage : string.Empty;
            }
        }

        #endregion
    }
}