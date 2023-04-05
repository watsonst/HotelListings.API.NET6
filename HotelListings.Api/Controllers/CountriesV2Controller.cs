using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListings.Api.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using HotelListings.API.Core.Contracts;
using HotelListings.API.Core.Models.Country;
using HotelListings.API.Core.Exceptions;

namespace HotelListings.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/countries")]
    [ApiVersion("2.0")]

    public class CountriesV2Controller : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesV2Controller(IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger) // ICRepo now injects db context
        {
            this._mapper = mapper;
            this._countriesRepository = countriesRepository;
            this._logger = logger;
        }

        // GET: api/Countries "action"
        //.../api/v2/Countries?$Select=name,shortname
        //...api/v2/Countries?$filter=name eq 'Cuba'
        //...api/v2/Countries?$orderby=name
        //...api/v2/Countries?$select=name&$orderby=name
        [HttpGet]
        [EnableQuery]//OData- can allow your users to search,sort,orderby,etc if you want them to be able to in the query
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries); //map to DTO
            return Ok(records);
        }


        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetails(id);
            
            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }
            
            var countryDto = _mapper.Map<CountryDto>(country);

            return Ok(countryDto);
        }


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

            var country = await _countriesRepository.GetAsync(id);
                
            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }

            _mapper.Map(updateCountryDto, country);

            try
            {
                await _countriesRepository.UpdateAsync(country);
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
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto) //only accpet bindings for name or shortname
        {
            var country = _mapper.Map<Country>(createCountryDto); //Doing the same thing in countryOld.

            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country); //return location/url in the repsonse headers "location:"
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);

            if (country == null)
            {
               throw new NotFoundException(nameof(GetCountry), id);
            }

            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
