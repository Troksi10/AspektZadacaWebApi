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
    public class ContactsController : ControllerBase
    {
        private readonly AspektBasicWebAPIDbContext _context;

        public ContactsController(AspektBasicWebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }
            return await _context.Contacts.ToListAsync();
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ContactWithCompanyAndCountryDto>> GetContact(int id)
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            var companyName = await _context.Companies
                .Where(c => c.Id == contact.CompanyId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            var countryName = await _context.Countries
                .Where(c => c.Id == contact.CountryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();

            var contactDto = new ContactWithCompanyAndCountryDto
            {
                Name = contact.Name,
                CompanyName = companyName,
                CountryName = countryName
            };

            return contactDto;
        }

        // GET: api/Contacts-filter
        [HttpGet("Contacts-filter")]
        public async Task<ActionResult<IEnumerable<ContactWithCompanyAndCountryDto>>> GetContacts(
            [FromQuery] int? countryId = null,
            [FromQuery] int? companyId = null)
        {
            IQueryable<Contact> contactsQuery = _context.Contacts;

            if (countryId.HasValue)
            {
                contactsQuery = contactsQuery.Where(c => c.CountryId == countryId.Value);
            }

            if (companyId.HasValue)
            {
                contactsQuery = contactsQuery.Where(c => c.CompanyId == companyId.Value);
            }

            var contacts = await contactsQuery.ToListAsync();

            var contactDtos = contacts.Select(contact => new ContactWithCompanyAndCountryDto
            {
                Name = contact.Name,
                CompanyName = _context.Companies.FirstOrDefault(c => c.Id == contact.CompanyId)?.Name,
                CountryName = _context.Countries.FirstOrDefault(c => c.Id == contact.CountryId)?.Name
            }).ToList();

            return contactDtos;
        }

        // PUT: api/Contacts/5
        [HttpPut("id")]
        public async Task<IActionResult> PutContact(int id, UpdateContactDto updateContactDto)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }


            if (!_context.Companies.Any(c => c.Id == updateContactDto.CompanyId))
            {
                ModelState.AddModelError(nameof(updateContactDto.CompanyId), "Invalid CompanyId");
                return BadRequest(ModelState);
            }

            contact.CompanyId = updateContactDto.CompanyId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(ContactDto contactDto)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'AspektZadacaDbContext.Contacts' is null.");
            }

            if (!_context.Companies.Any(c => c.Id == contactDto.CompanyId))
            {
                ModelState.AddModelError(nameof(contactDto.CompanyId), "Invalid CompanyId");
                return BadRequest(ModelState);
            }

            if (!_context.Countries.Any(c => c.Id == contactDto.CountryId))
            {
                ModelState.AddModelError(nameof(contactDto.CountryId), "Invalid CountryId");
                return BadRequest(ModelState);
            }

            var contact = new Contact
            {
                Name = contactDto.Name,
                CompanyId = contactDto.CompanyId,
                CountryId = contactDto.CountryId,
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok(true);
        }



        [HttpDelete("id")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(int id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}