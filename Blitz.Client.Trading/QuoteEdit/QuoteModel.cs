using Blitz.Common.Trading.Quote.Edit;

namespace Blitz.Client.Trading.QuoteEdit
{
    public class QuoteModel : ModelWithValidation<QuoteModel, QuoteValidation>
    {
        public long Id { get; private set; }

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

        public QuoteModel(long id)
        {
            Id = id;
        }
    }
}