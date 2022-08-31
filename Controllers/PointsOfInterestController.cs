using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using City_info.Models;
using City_info.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace City_info.Controllers
{
    //[Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities/{cityid}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase

    {
        readonly public ILogger<PointsOfInterestController> _logger;
        readonly public IMailService _mailService;
  
        private readonly IMapper mapper;
        private readonly ICityInfoRepository cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger , IMailService _localMailService, IMapper mapper, ICityInfoRepository cityInfoRepository) {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService= _localMailService ?? throw new ArgumentNullException(nameof(_localMailService));
          
            this.mapper = mapper;
            this.cityInfoRepository = cityInfoRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDto>>> GetPointsOfInterest(int cityid)
        {
            var cityexsist = await cityInfoRepository.CityExsistsAsync(cityid);
            if (!cityexsist)
            {
                return NotFound();
            }
            var pointsofinterest= await cityInfoRepository.GetPointsOfInterestAsync(cityid);
            return Ok(mapper.Map<IEnumerable<PointsOfInterestDto>>(pointsofinterest));
        }
        [HttpGet("{pointofinterestid}",Name = "GetPointsOfInterest")]
        public async Task<ActionResult<PointsOfInterestDto>> GetPointOfInterest(int cityid , int pointofinterestid)
        {
            var cityexsist = await cityInfoRepository.CityExsistsAsync(cityid);
            if (!cityexsist)
            {
                return NotFound();
            }
            var pointofinterest = await cityInfoRepository.GetPointOfInterestAsync(cityid, pointofinterestid);
            if (pointofinterest == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PointsOfInterestDto>(pointofinterest));
        }
        [HttpPost]
        public async Task<ActionResult<PointsOfInterestDto>> CreatePointOfInterest(int cityid, PointsOfInterestCreationDto pointofinterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var cityexsist = await cityInfoRepository.CityExsistsAsync(cityid);
            if (!cityexsist)
            {
                return NotFound();
            }
            var finalpointofinterest = mapper.Map<Entites.PointsOfInterest>(pointofinterest);
            await cityInfoRepository.AddPointOfInterestForCityAsync(cityid, finalpointofinterest);
            await cityInfoRepository.SavechangesAsync();
            var topointsofinterest=mapper.Map<PointsOfInterestDto>(finalpointofinterest);
            return CreatedAtRoute("GetPointsOfInterest",new
            {
                cityid =cityid ,
                pointofinterestid = topointsofinterest.Id
            },
            topointsofinterest);
        }
        [HttpPut("{pointofinterestid}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityid, int pointofinterestid, PointsOfInterestUpdateDto pointsOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var cityExs = await cityInfoRepository.CityExsistsAsync(cityid);
            if (!cityExs)
            {
                return NotFound();
            }
            var pointofinterest = await cityInfoRepository.GetPointOfInterestAsync(cityid, pointofinterestid);
            if (pointofinterest == null)
            {
                return NotFound();
            }
            mapper.Map(pointsOfInterest, pointofinterest);
            await cityInfoRepository.SavechangesAsync();

            return NoContent();
        }
        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> partiallyUpdatePointOfInterest(int cityid, int pointofinterestid, JsonPatchDocument<PointsOfInterestUpdateDto> patchDocument)
        {
            var cityExs = await cityInfoRepository.CityExsistsAsync(cityid);
            if (!cityExs)
            {
                return NotFound();
            }
            var pointofinterest = await cityInfoRepository.GetPointOfInterestAsync(cityid, pointofinterestid);
            if (pointofinterest == null)
            {
                return NotFound();
            }
            var newPointsOfInterest=mapper.Map<PointsOfInterestUpdateDto>(pointofinterest); 
            patchDocument.ApplyTo(newPointsOfInterest,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!TryValidateModel(newPointsOfInterest))
            {
                return BadRequest();
            }
            mapper.Map(newPointsOfInterest,pointofinterest);
            await cityInfoRepository.SavechangesAsync();
            return NoContent();
        }
        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityid, int pointofinterestid)
        {
            var cityExs = await cityInfoRepository.CityExsistsAsync(cityid);
            if (!cityExs)
            {
                return NotFound();
            }
            var pointofinterest = await cityInfoRepository.GetPointOfInterestAsync(cityid, pointofinterestid);
            if (pointofinterest == null)
            {
                return NotFound();
            }
            cityInfoRepository.DeletePointsOfInterest(pointofinterest);
            await cityInfoRepository.SavechangesAsync();
            _mailService.send("Points of inerest deleted", "Poit of interest is just deleted");

            return NoContent();
        }
    }
}