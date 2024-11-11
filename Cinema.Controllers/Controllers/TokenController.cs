using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ApiControllerBase
    {
        private readonly IServiceManager _service;
        public TokenController(IServiceManager service) => _service = service;

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var baseResult = await _service.AuthenticationService.RefreshToken(tokenDto);
            
            var tokenDtoToReturn = baseResult.GetResult<TokenDto>();

            return Ok(tokenDtoToReturn);
        }
    }
}