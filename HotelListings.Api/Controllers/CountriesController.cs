using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListings.Api.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using HotelListings.API.Core.Contracts;
using HotelListings.API.Core.Models.Country;
using HotelListings.API.Core.Models;
using HotelListings.API.Core.Exceptions;

namespace HotelListings.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/countries")]
    [ApiVersion("1.0", Deprecated = true)]

    public class CountriesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger) // ICRepo now injects db context
        {
            this._mapper = mapper;
            this._countriesRepository = countriesRepository;
            this._logger = logger;
        }

        // GET: api/Countries/GetAll "action"
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepository.GetAllAsync<GetCountryDto>();//using the generic Tresult GetAllAsync, where mapping is done and a dto is being returned already
            return Ok(countries);
        }

        // GET: api/Countries/?StartIndex=0&pagesize=25&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            var pagedCountriesResult = await _countriesRepository.GetAllAsync<CountryDto>(queryParameters);//mapping done at DB level in genericRepo.cs
            return Ok(pagedCountriesResult);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetails(id);
            return Ok(country);
        }

        //// GET: api/Countries/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CountryDto>> GetCountry(int id)
        //{
        //    var country = await _countriesRepository.GetDetails(id);//if mapped at db level in genericRepo.cs, get details would return countrydto and 65 mapping not needed
        //    if (country == null)
        //    {
        //        throw new NotFoundException(nameof(GetCountry), id);
        //    }
        //    var countryDto = _mapper.Map<CountryDto>(country);
        //    return Ok(countryDto);
        //}


        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                throw new BadRequestException(nameof(PutCountry), updateCountryDto.Id);
            }
            try
            {
                await _countriesRepository.UpdateAsync(id, updateCountryDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    throw new NotFoundException(nameof(CountryExists), id);
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }


        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto createCountryDto)
        {
            var country = await _countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountryDto);//want the get country returned after created.
            return CreatedAtAction((nameof(GetCountry)), new { id = country.Id }, country); //return location/url in the repsonse headers "location:"
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            await _countriesRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
