using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspektZadacaWebApi.Data;
using AspektZadacaWebApi.Dtos;

namespace AspektZadacaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly AspektBasicWebAPIDbContext _context;

        public CompaniesController(AspektBasicWebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            if (_context.Companies == null)
            {
                return NotFound();
            }
            return await _context.Companies.ToListAsync();
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, CompanyDto companyDto)
        {
            var existingCompany = await _context.Companies.FindAsync(id);

            if (existingCompany == null)
            {
                return NotFound();
            }

            existingCompany.Name = companyDto.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyDto.Name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(true);
        }
        private bool CompanyExists(string name)
        {
            throw new NotImplementedException();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(CompanyDto companyDto)
        {
            var company = new Company();
            company.Name = companyDto.Name;

            if (_context.Companies == null)
            {
                return Problem("Entity set 'AspektZadacaWebApiDbContext.Countries'  is null.");
            }
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (_context.Companies == null)
            {
                return NotFound();
            }
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
