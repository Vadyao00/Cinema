namespace Cinema.Domain.Responses
{
    public sealed class RefreshTokenBadRequestResponse : ApiBadRequestResponse
    {
        public RefreshTokenBadRequestResponse() : base("Invalid client request. The tokenDto has some invalid values.")
        {
        }
    }
}