using Cinema.Domain.Responses;

namespace Cinema.Controllers.Extensions
{
    public static class ApiBaseResponseExtention
    {
        public static TResultType GetResult<TResultType>(this ApiBaseResponse apiBaseResponse)
            => ((ApiOkResponse<TResultType>)apiBaseResponse).Result;
    }
}