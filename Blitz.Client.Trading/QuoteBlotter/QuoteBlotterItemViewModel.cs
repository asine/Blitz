using System;

using Blitz.Common.Trading.Quote;

using Microsoft.Practices.Prism.ViewModel;

namespace Blitz.Client.Trading.QuoteBlotter
{
    public class QuoteBlotterItemViewModel : NotificationObject
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

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}