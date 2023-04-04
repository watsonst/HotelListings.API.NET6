﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListings.Api.Data;
using HotelListings.Api.Models.Country;
using AutoMapper;
using HotelListings.Api.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListings.Api.Exceptions;
using HotelListings.Api.Models;

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
            var countries = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countries); //map to DTO
            return Ok(records);
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

            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }

            var countryDto = _mapper.Map<CountryDto>(country);

            return Ok(countryDto);
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
