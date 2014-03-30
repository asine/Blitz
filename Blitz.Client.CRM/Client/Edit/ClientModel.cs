using System;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;
using Naru.WPF.ViewModel;

namespace Blitz.Client.CRM.Client.Edit
{
    public class ClientModel : ModelWithValidationAsync<ClientModel, ClientValidator>
    {
        #region FirstName

        private readonly ObservableProperty<string> _firstName = new ObservableProperty<string>();

        public string FirstName
        {
            get { return _firstName.Value; }
            set { _firstName.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region LastName

        private readonly ObservableProperty<string> _lastName = new ObservableProperty<string>();

        public string LastName
        {
            get { return _lastName.Value; }
            set { _lastName.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region FullName

        private readonly ObservableProperty<string> _fullName = new ObservableProperty<string>();

        public string FullName
        {
            get { return _fullName.Value; }
            set { _fullName.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region Gender

        private readonly ObservableProperty<string> _gender = new ObservableProperty<string>();

        public string Gender
        {
            get { return _gender.Value; }
            set { _gender.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region AddressLine1

        private readonly ObservableProperty<string> _addressLine1 = new ObservableProperty<string>();

        public string AddressLine1
        {
            get { return _addressLine1.Value; }
            set { _addressLine1.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region AddressLine2

        private readonly ObservableProperty<string> _addressLine2 = new ObservableProperty<string>();

        public string AddressLine2
        {
            get { return _addressLine2.Value; }
            set { _addressLine2.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region PostCode

        private readonly ObservableProperty<string> _postCode = new ObservableProperty<string>();

        public string PostCode
        {
            get { return _postCode.Value; }
            set { _postCode.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region Country

        private readonly ObservableProperty<string> _country = new ObservableProperty<string>();

        public string Country
        {
            get { return _country.Value; }
            set { _country.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region DateOfBirth

        private readonly ObservableProperty<DateTime> _dateOfBirth = new ObservableProperty<DateTime>();

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth.Value; }
            set { _dateOfBirth.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public ClientModel(IDispatcherSchedulerProvider scheduler, IValidationAsync<ClientModel, ClientValidator> validation)
            : base(scheduler, validation)
        {
            _firstName.ConnectINPCProperty(this, () => FirstName, scheduler).AddDisposable(Disposables);
            _firstName.AddValidation(Validation, scheduler, () => FirstName).AddDisposable(Disposables);

            _lastName.ConnectINPCProperty(this, () => LastName, scheduler).AddDisposable(Disposables);
            _lastName.AddValidation(Validation, scheduler, () => LastName).AddDisposable(Disposables);

            _fullName.ConnectINPCProperty(this, () => FullName, scheduler).AddDisposable(Disposables);

            _gender.ConnectINPCProperty(this, () => Gender, scheduler).AddDisposable(Disposables);
            _gender.AddValidation(Validation, scheduler, () => Gender).AddDisposable(Disposables);

            _addressLine1.ConnectINPCProperty(this, () => AddressLine1, scheduler).AddDisposable(Disposables);
            _addressLine1.AddValidation(Validation, scheduler, () => AddressLine1).AddDisposable(Disposables);

            _addressLine2.ConnectINPCProperty(this, () => AddressLine2, scheduler).AddDisposable(Disposables);
            _addressLine2.AddValidation(Validation, scheduler, () => AddressLine2).AddDisposable(Disposables);

            _postCode.ConnectINPCProperty(this, () => PostCode, scheduler).AddDisposable(Disposables);
            _postCode.AddValidation(Validation, scheduler, () => PostCode).AddDisposable(Disposables);

            _country.ConnectINPCProperty(this, () => Country, scheduler).AddDisposable(Disposables);
            _country.AddValidation(Validation, scheduler, () => Country).AddDisposable(Disposables);

            _dateOfBirth.ConnectINPCProperty(this, () => DateOfBirth, scheduler).AddDisposable(Disposables);
            _dateOfBirth.AddValidation(Validation, scheduler, () => DateOfBirth).AddDisposable(Disposables);

            ObservableEx.WhenAny(_firstName.ValueChanged, _lastName.ValueChanged)
                        .Subscribe(x => FullName = string.Format("{0} {1}", x.Item1, x.Item2))
                        .AddDisposable(Disposables);

            FirstName = string.Empty;
            LastName = string.Empty;
        }
    }
}