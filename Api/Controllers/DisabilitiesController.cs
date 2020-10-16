using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Data.Models;
using DataModels.Responses;
using AutoMapper;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisabilitiesController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public DisabilitiesController(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Disabilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisabilityResponse>>> GetDisability() =>
            _mapper.Map<IEnumerable<DisabilityResponse>>(await _context.Disability.ToListAsync()).ToList();

        // GET: api/Disabilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisabilityResponse>> GetDisability(int id)
        {
            var disability = await _context.Disability.FindAsync(id);

            if (disability == null)
            {
                return NotFound();
            }

            return _mapper.Map<DisabilityResponse>(disability);
        }

        // PUT: api/Disabilities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisability(int id, Disability disability)
        {
            if (id != disability.DisabilityId)
            {
                return BadRequest();
            }

            _context.Entry(disability).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisabilityExists(id))
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

        // POST: api/Disabilities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DisabilityResponse>> PostDisability(Disability disability)
        {
            _context.Disability.Add(disability);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDisability", new { id = disability.DisabilityId }, disability);
        }

        // DELETE: api/Disabilities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DisabilityResponse>> DeleteDisability(int id)
        {
            var disability = await _context.Disability.FindAsync(id);
            if (disability == null)
            {
                return NotFound();
            }

            _context.Disability.Remove(disability);
            await _context.SaveChangesAsync();

            return _mapper.Map<DisabilityResponse>(disability);
        }

        private bool DisabilityExists(int id)
        {
            return _context.Disability.Any(e => e.DisabilityId == id);
        }
    }
}
