using LWS.Dto;
using LWS.GenericResponse;
using LWS.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserDto userDto)
        {
            var result = await _authService.LoginUser(userDto);
            if(result.Item1==0)
            {
                return NotFound(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Messege));
            }
            if (result.Item1 == 2)
            {
                return BadRequest(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Messege));
            }

            return Ok(ResponseResult<TokenDto>.Success(result.Item2, result.Item2.Messege));
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserDto userDto)
        {
            try
            {
                var result = await _authService.RegisterUser(userDto);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }

                if(result.Item1==1)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
