using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParkAPI_1033.Models;
using NationalParkAPI_1033.Models.DTOs;
using NationalParkAPI_1033.Repository.IRepository;

namespace NationalParkAPI_1033.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository,IMapper mapper)
        {
            _mapper = mapper;
            _trailRepository = trailRepository;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().Select(_mapper.Map<TrailDto>));
        }
        [HttpGet("{trailid:int}",Name = "GetTrail")]
        public IActionResult GetTrail(int trailid)
        {
            var trail=_trailRepository.GetTrail(trailid);
            if(trail== null) return NotFound();
            var trailDto=_mapper.Map<Trail,TrailDto>(trail);
            if(trailDto==null) return NotFound();
            return Ok(trailDto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            if(trailDto==null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail in db ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var trail = _mapper.Map<TrailDto, Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", "Unable to create trail ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new {trailId= trail.Id},trail);
        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody] TrailDto trailDto)
        {
            if(trailDto== null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest();
            var trail=_mapper.Map<TrailDto, Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", "Unable to Update trail ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int  trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
                return NotFound();
            var trail = _trailRepository.GetTrail(trailId);
            if(trail == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", "Unable to Delete trail ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
