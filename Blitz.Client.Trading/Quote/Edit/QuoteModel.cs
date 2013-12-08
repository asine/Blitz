using System;

using Blitz.Common.Trading.Quote.Edit;

using Naru.RX;
using Naru.WPF.Scheduler;
using Naru.WPF.Validation;
using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteModel : ModelWithValidation<QuoteModel, QuoteValidation>
    {
        public Guid Id { get; private set; }

        public string QuoteReference { get; set; }

        #region Instrument

        private readonly ObservableProperty<LookupValue> _instrument = new ObservableProperty<LookupValue>();

        public LookupValue Instrument
        {
            get { return _instrument.Value; }
            set { this.RaiseAndSetIfChanged(_instrument, value); }
        }

        #endregion

        #region Notes

        private readonly ObservableProperty<string> _notes = new ObservableProperty<string>();

        public string Notes
        {
            get { return _notes.Value; }
            set { this.RaiseAndSetIfChanged(_notes, value); }
        }

        #endregion

        public QuoteModel(ISchedulerProvider scheduler)
        {
            _instrument.ConnectINPCProperty(this, () => Instrument, scheduler).AddDisposable(Disposables);
            _notes.ConnectINPCProperty(this, () => Notes, scheduler).AddDisposable(Disposables);
        }

        public void Initialise(Guid id)
        {
            Id = id;
        }
    }
}