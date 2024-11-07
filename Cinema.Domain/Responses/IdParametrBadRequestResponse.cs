namespace Cinema.Domain.Responses
{
    public sealed class IdParametrBadRequestResponse : ApiBadRequestResponse
    {
        public IdParametrBadRequestResponse() : base("Parametr ids is null")
        {
        }
    }
}