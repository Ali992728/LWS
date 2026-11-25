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
        public async Task<IActionResult> DeleteLaptop([FromRoute]string serialNo)
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


        [HttpGet("GetAllLaptops")]
        public async Task<IActionResult> GetAllLaptops()
        {
            try
            {
                var result = await _laptopService.GetAllLaptops();
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<List<LaptopDto>>.Failure(null, "No Laptop Found."));
                }
                return Ok(ResponseResult<List<LaptopDto>>.Success(result.Item2, "Laptops Retrieved Successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("GetAllLaptopsByBrand/{brand}")]
        public async Task<IActionResult> GetAllLaptopsByBrand([FromRoute]string brand)
        {
            try
            {
                var result = await _laptopService.GetAllLaptopsByBrand(brand);

                return Ok(ResponseResult<List<LaptopDto>>.Success(result.Item2,"Laptops Retrieved Successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("GetAllLaptopsByModel/{model}")]
        public async Task<IActionResult> GetAllLaptopByModel([FromRoute]string model)
        {
            try
            {
                var result = await _laptopService.GetAllLaptopsByModel(model);

                return Ok(ResponseResult<List<LaptopDto>>.Success(result.Item2,"Laptops Retrieved Successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet("GetLaptopBySerialNo/{serialNo}")]
        public async Task<IActionResult> GetLaptopBySerialNo([FromRoute] string serialNo)
        {
            try
            {
                var result = await _laptopService.GetLaptopBySerialNo(serialNo);

                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<LaptopDto>.Failure(null, "Laptop Not Found."));
                }

                return Ok(ResponseResult<LaptopDto>.Success(result.Item2, "Laptop Retrieved Successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet("GetAllLaptopsByPriceRange/{minPrice}/{maxPrice}")]
        public async Task<IActionResult> GetAllLaptopsByPriceRange([FromRoute] decimal minPrice,[FromRoute] decimal maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < minPrice)
                {
                    return BadRequest(ResponseResult<List<LaptopDto>>.Failure(null, "Invalid Price Range."));
                }

                var result = await _laptopService.GetAllLaptopsByPriceRange(minPrice, maxPrice);

                return Ok(ResponseResult<List<LaptopDto>>.Success(
                    result.Item2,
                    "Laptops Retrieved Successfully."
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
