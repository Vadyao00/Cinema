namespace Cinema.Domain.Responses
{
    public sealed class TimeRangeBadRequestResponse : ApiBadRequestResponse
    {
        public TimeRangeBadRequestResponse() : base("EndTime must be less than StartTime")
        {
        }
    }
}