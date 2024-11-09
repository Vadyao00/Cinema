namespace Cinema.Domain.RequestFeatures
{
    public class EventParameters : RequestParameters
    {
        public decimal MinTicketPrice { get; set; } = (decimal)0.0;
        public decimal MaxTicketPrice { get; set; } = 92233720368547;
        public TimeOnly StartTime {  get; set; } = new TimeOnly(0,0,0);
        public TimeOnly EndTime {  get; set; } = new TimeOnly(23,59,59);

        public bool ValidTicketPriceRange => MaxTicketPrice > MinTicketPrice;
        public bool ValidTimeRange => EndTime > StartTime;
    }
}