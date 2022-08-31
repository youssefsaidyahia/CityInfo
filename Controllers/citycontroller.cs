using AutoMapper;
using City_info.Models;
using City_info.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace City_info.Controllers
{
    [ApiController]
    //[Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/Cities")]
    public class citycontroller : ControllerBase
    {

        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;
        const int maxpageSize= 20;

        public citycontroller(ICityInfoRepository cityInfoRepository , IMapper mapper)
        {
            this.cityInfoRepository = cityInfoRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<cityWithoutPointsOfIntereDto>>> Getcities(string? name,string? searchquery,int pageNumber=1 ,int pagesize=10)
        {
            if(pagesize > maxpageSize)
            {
                pagesize = maxpageSize;
            }
            var cityList = await cityInfoRepository.GetCitiesAsync(name,searchquery,pageNumber,pagesize);

            return Ok(mapper.Map<IEnumerable<cityWithoutPointsOfIntereDto>>(cityList));
        }
        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">The id of the city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include the points of interest</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested city</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>GetcityByid(int id  , bool includepointsofinterest=false)
           {
            var city = await cityInfoRepository.GetCityAsync(id, includepointsofinterest);
               if(city == null)
               {
                   return NotFound();
               }
            if (includepointsofinterest)
            {
                return Ok(mapper.Map<cityDto>(city));
            }
            return Ok(mapper.Map<cityWithoutPointsOfIntereDto>(city));
        }
    }
}

