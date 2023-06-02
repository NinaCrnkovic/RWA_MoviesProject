using AutoMapper;
using BL.BLModels;
using BL.Repositories;
using IntegrationModule.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepo;

        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepo, IMapper mapper)
        {
            _countryRepo = countryRepo;
            _mapper = mapper;
        }
        // GET: api/<CountryController>
        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetAll()
        {
            try
            {
                var blCountry = _countryRepo.GetAll();
                var country = _mapper.Map<IEnumerable<Country>>(blCountry);
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested" + ex);
            }
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<Country> GetCountryById(int id)
        {
            try
            {
                var blCountry = _countryRepo.GetById(id);
                if (blCountry == null)
                    return NotFound($"Could not find country with id {id}");

                var country = _mapper.Map<Country>(blCountry);
                return Ok(country);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }


        [HttpPost("[action]")]
        [ProducesResponseType(typeof(Country), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string[]>), 400)]
        public ActionResult<Country> CreateCountry([FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                BLCountry country = new BLCountry
                {
                    Name = name,
                    Code = code
                };
                //country.Id = 0;
                var blCountry = _mapper.Map<BLCountry>(country);
                var newCountry = _countryRepo.Add(blCountry);
                var createdCountry = _mapper.Map<Country>(newCountry);

                return Ok(createdCountry);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the genre.");
            }
        }

        [HttpPut("[action]/{id}")]
        public ActionResult<Country> UpdateCountry(int id, Country country)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var blCountry = _mapper.Map<BLCountry>(country);
                var updatedCountry = _countryRepo.Update(id, blCountry);
                var modifiedCountry = _mapper.Map<Country>(updatedCountry);

                return Ok(modifiedCountry);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the country.");
            }
        }


        [HttpDelete("[action]/{id}")]
        public ActionResult<Country> DeleteCountry(int id)
        {
            try
            {
                var blCountry = _countryRepo.GetById(id);
                if (blCountry == null)
                    return NotFound($"Could not find country with id {id}");

                _countryRepo.Delete(id);
                return Ok(blCountry);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the country." + ex);
            }
        }
    }
}

