namespace Cinema.Domain.RequestFeatures
{
    public class SeatParameters : RequestParameters
    {
        public SeatParameters() => OrderBy = "SeatNumber";
        public uint MinSeatNumber { get; set; }
        public uint MaxSeatNumber { get; set; } = int.MaxValue;

        public bool ValidSeatNumber => MaxSeatNumber > MinSeatNumber;
        public string? searchEventName { get; set; }
        public string? searchShowtimeName { get; set; }
    }
}