using System;

using Blitz.Common.Trading.Quote;

using Naru.WPF.ViewModel;

namespace Blitz.Client.Trading.Quote.Blotter
{
    public class QuoteBlotterItemViewModel : NotifyPropertyChanged
    {
        public Guid Id { get; set; }

        public string QuoteReference { get; set; }

        public QuoteStatus Status { get; set; }

        public string Instrument { get; set; }

        public decimal? BestBid { get; set; }

        public decimal? BestOffer { get; set; }

        public decimal? BestBidVol { get; set; }

        public decimal? BestOfferVol { get; set; }

        public string Notes { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}