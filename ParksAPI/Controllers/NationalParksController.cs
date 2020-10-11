using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParksAPI.Models;
using ParksAPI.Models.Dtos;
using ParksAPI.Repository.IRepository;

namespace ParksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NationalParksController : Controller

    {
        private readonly INationalParkRepository npRepo;
        private readonly IMapper mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            this.npRepo = npRepo;
            this.mapper = mapper;
        }
         
        /// <summary>
        /// Get List of National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var objList = this.npRepo.GetNationalParks();

            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(this.mapper.Map<NationalParkDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get individual  national park
        /// </summary>
        /// <param name="nationalParkId"> The Id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}",Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = this.npRepo.GetNationalPark(nationalParkId);

            if(obj== null )
            {
                return NotFound();
            }
            var objDto = this.mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (nationalParkDto == null)

            {
                return BadRequest(ModelState);
            }

            if (this.npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("","National Park Exists");
                return StatusCode(404, ModelState);
            }
            var nationalParkObj = this.mapper.Map<NationalPark>(nationalParkDto);
            if(!this.npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("",$"Something Went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj) ;

        }
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)    
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = this.mapper.Map<NationalPark>(nationalParkDto);
            if (!this.npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something Went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!this.npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = this.npRepo.GetNationalPark(nationalParkId);
            if (!this.npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something Went wrong when deleteing the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
