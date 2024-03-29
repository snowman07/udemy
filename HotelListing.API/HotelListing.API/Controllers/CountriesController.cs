﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using AutoMapper;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController( IMapper mapper, ICountriesRepository countriesRepository, ILogger<CountriesController> logger)
        {
            this._mapper = mapper;
            this._countriesRepository = countriesRepository;
            this._logger = logger;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
          //if (_countriesRepository.GetAllAsync() == null)
          //{
          //    return NotFound();
          //}
            var countries = await _countriesRepository.GetAllAsync(); //Same as Select * from Countries
            var records = _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(records); 
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
          //if (_countriesRepository.GetDetails(id) == null)
          //{
          //    return NotFound();
          //}
            //var country = await _context.Countries.Include(q => q.Hotels)
            //    .FirstOrDefaultAsync(q => q.Id == id);
            var country = await _countriesRepository.GetDetails(id);

            if (country == null)
            {
                //_logger.LogWarning($"Record found in {nameof(GetCountry)} with ID: {id}");
                //return NotFound();
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
                return BadRequest("Invalid Record Id");
            }

            //_context.Entry(country).State = EntityState.Modified;
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                //return NotFound();
                throw new NotFoundException(nameof(GetCountries), id);
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
                    return NotFound();
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
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {
            //if (await _countriesRepository.AddAsync() == null)
            //{
            //    return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
            //}

            //var countryOld = new Country
            //{
            //    Name = createCountryDto.Name,
            //    ShortName = createCountryDto.ShortName,
            //};

            var country = _mapper.Map<Country>(createCountryDto); // efficiently converting one data type to another

            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            //if (_countriesRepository.DeleteAsync == null)
            //{
            //    return NotFound();
            //}
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                //return NotFound();
                throw new NotFoundException(nameof(GetCountries), id);
            }

            //_context.Countries.Remove(country);
            //await _context.SaveChangesAsync();
            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            //return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
            return await _countriesRepository.Exists(id);
        }
    }
}
