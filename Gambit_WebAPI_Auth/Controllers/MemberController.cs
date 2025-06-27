using Gambit_WebAPI_Auth.Contracts.Input;
using Gambit_WebAPI_Auth.Contracts.Output;
using Gambit_WebAPI_Auth.Domain.Models;
using Gambit_WebAPI_Auth.Handlers;
using Gambit_WebAPI_Auth.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gambit_WebAPI_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class MemberController : ControllerBase
    {
        private readonly MemberHandler _memberHandler;
        private readonly TokenHelper _tokenHelper;
        public MemberController(MemberHandler memberHandler, TokenHelper tokenHelper)
        {
            _memberHandler = memberHandler;
            _tokenHelper = tokenHelper;
        }

        [HttpPost("login")]
        [ProducesResponseType<MemberTokenOutput>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] MemberLoginInput memberLogin)
        {
            Member member = await _memberHandler.Login(memberLogin);
            string token = _tokenHelper.GenerateToken(member);

            return Ok(new MemberTokenOutput()
            {
                Token = token
            });
        }

        [HttpPost("register")]
        [ProducesResponseType<MemberTokenOutput>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] MemberRegisterInput memberRegister)
        {
            Member memberCreated = await _memberHandler.Register(memberRegister);
            string token = _tokenHelper.GenerateToken(memberCreated);

            return Ok(new MemberTokenOutput()
            {
                Token = token
            });
        }
    }
}
