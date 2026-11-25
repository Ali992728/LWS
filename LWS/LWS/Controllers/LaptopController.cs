using LWS.Dto;
using LWS.GenericResponse;
using LWS.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController(ILaptopService _laptopService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("AddLaptop")]
        public async Task<IActionResult> AddLaptop([FromBody] LaptopDto dto)
        {
            try
            {
                var result = await _laptopService.AddLaptop(dto);

                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateLaptop")]
        public async Task<IActionResult> UpdateLaptop([FromBody] LaptopDto dto)
        {
            try
            {
                var result = await _laptopService.UpdateLaptop(dto);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteLaptop/{serialNo}")]
        public async Task<IActionResult> DeleteLaptop(string serialNo)
        {
            try
            {
                var result = await _laptopService.DeleteLaptop(serialNo);

                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null,result.Item2));
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
