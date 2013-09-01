namespace Blitz.Common.Trading.Quote
{
    public class QuoteDto
    {
        public long Id { get; set; }

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