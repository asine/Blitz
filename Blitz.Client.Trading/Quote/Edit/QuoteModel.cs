using System;

using Blitz.Common.Trading.Quote.Edit;

namespace Blitz.Client.Trading.Quote.Edit
{
    public class QuoteModel : ModelWithValidation<QuoteModel, QuoteValidation>
    {
        public Guid Id { get; private set; }

        public string QuoteReference { get; set; }

        #region Instrument

        private LookupValue _instrument;

        public LookupValue Instrument
        {
            get { return _instrument; }
            set
            {
                if (Equals(value, _instrument)) return;
                _instrument = value;
                RaisePropertyChanged(() => Instrument);
            }
        }

        #endregion

        #region Notes

        private string _notes;

        public string Notes
        {
            get { return _notes; }
            set
            {
                if (value == _notes) return;
                _notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }

        #endregion

        public QuoteModel(Guid id)
        {
            Id = id;
        }
    }
}