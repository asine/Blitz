using System;

using Blitz.Common.Trading.Quote.Edit;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteModel : ModelWithValidationAsync<QuoteModel, QuoteValidator>
    {
        public Guid Id { get; private set; }

        public string QuoteReference { get; set; }

        #region Instrument

        private readonly ObservableProperty<LookupValue> _instrument = new ObservableProperty<LookupValue>();

        public LookupValue Instrument
        {
            get { return _instrument.Value; }
            set { _instrument.RaiseAndSetIfChanged(value); }
        }

        #endregion

        #region Notes

        private readonly ObservableProperty<string> _notes = new ObservableProperty<string>();

        public string Notes
        {
            get { return _notes.Value; }
            set { _notes.RaiseAndSetIfChanged(value); }
        }

        #endregion

        public QuoteModel(ISchedulerProvider scheduler, IValidationAsync<QuoteModel, QuoteValidator> validation)
            : base(scheduler, validation)
        {
            _instrument.ConnectINPCProperty(this, () => Instrument, scheduler).AddDisposable(Disposables);
            _instrument.AddValidation(validation, scheduler, () => Instrument).AddDisposable(Disposables);

            _notes.ConnectINPCProperty(this, () => Notes, scheduler).AddDisposable(Disposables);
            _notes.AddValidation(validation, scheduler, () => Notes).AddDisposable(Disposables);
        }

        public void Initialise(Guid id)
        {
            Id = id;
        }
    }
}