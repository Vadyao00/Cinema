namespace Cinema.Domain.Responses
{
    public sealed class AgeRestrictionBadRequestResponse : ApiBadRequestResponse
    {
        public AgeRestrictionBadRequestResponse() : base("Min age must be less than Max age")
        {
        }
    }
}