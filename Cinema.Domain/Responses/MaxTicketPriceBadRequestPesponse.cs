namespace Cinema.Domain.Responses
{
    public sealed class MaxTicketPriceBadRequestPesponse : ApiBadRequestResponse
    {
        public MaxTicketPriceBadRequestPesponse() : base($"MinTicketPrice must be less than MaxTicketPrice")
        {
        }
    }
}