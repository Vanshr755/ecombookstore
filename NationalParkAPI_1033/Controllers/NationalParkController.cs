using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NationalParkAPI_1033.Models;
using NationalParkAPI_1033.Models.DTOs;
using NationalParkAPI_1033.Repository.IRepository;

namespace NationalParkAPI_1033.Controllers
{
    [Route("api/NationalPark")]
   
    [ApiController]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalparkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository,IMapper mapper)
        {
            _mapper = mapper;
            _nationalparkRepository = nationalParkRepository;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkDtoList = _nationalparkRepository.GetNationalParks().Select
                (_mapper.Map<NationalParkDto>);
            return Ok(nationalParkDtoList);//200
        }
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark=_nationalparkRepository.GetNationalPark(nationalParkId);
            if(nationalPark == null) return NotFound();
            var nationalParkDto=_mapper.Map<NationalPark,NationalParkDto>(nationalPark);
            if(nationalParkDto == null) return NotFound();
            return Ok(nationalParkDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null) return BadRequest(); 
            if(!ModelState.IsValid) return BadRequest();
            var nationalPark=_mapper.Map<NationalParkDto,NationalPark>(nationalParkDto);
            if (_nationalparkRepository.NationalParkExists(nationalPark.Name))
            {
                ModelState.AddModelError("", "National Park In Db!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!_nationalparkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Unable to create National Park!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id }, 
                nationalPark);
        }
        [HttpPut]//update 
        public IActionResult UpdateNationalPark([FromBody]NationalParkDto nationalParkDto)
        {
            if(nationalParkDto==null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var nationalPark=_mapper.Map<NationalParkDto,NationalPark>(nationalParkDto);
            if (!_nationalparkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Unable to Update National Park");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{nationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!_nationalparkRepository.NationalParkExists(nationalParkId))
                return NotFound();
            var nationalPark = _nationalparkRepository.GetNationalPark(nationalParkId);
            if(nationalPark==null) return NotFound();
            if (!_nationalparkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Unable to delete national park!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return  Ok();
        }
    }
}
