using System;

namespace Blitz.Common.Trading.Quote
{
    public class QuoteDto
    {
        public Guid Id { get; set; }

        public string QuoteReference { get; set; }

        public QuoteStatus Status { get; set; }

        public long InstrumentId { get; set; }
        public string InstrumentName { get; set; }

        public decimal? BestBid { get; set; }

        public decimal? BestOffer { get; set; }

        public string Notes { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}