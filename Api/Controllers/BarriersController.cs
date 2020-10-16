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
    public class BarriersController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;

        public BarriersController(MainContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Barriers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarrierResponse>>> GetBarriers() =>
            _mapper.Map<IEnumerable<BarrierResponse>>(await _context.Barriers.ToListAsync()).ToList();

        // GET: api/Barriers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BarrierResponse>> GetBarrier(int id)
        {
            var barrier = await _context.Barriers.FindAsync(id);

            if (barrier == null)
            {
                return NotFound();
            }

            return _mapper.Map<BarrierResponse>(barrier);
        }

        // PUT: api/Barriers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBarrier(int id, Barrier barrier)
        {
            if (id != barrier.BarrierId)
            {
                return BadRequest();
            }

            _context.Entry(barrier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarrierExists(id))
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

        // POST: api/Barriers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BarrierResponse>> PostBarrier(Barrier barrier)
        {
            _context.Barriers.Add(barrier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBarrier", new { id = barrier.BarrierId }, barrier);
        }

        // DELETE: api/Barriers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BarrierResponse>> DeleteBarrier(int id)
        {
            var barrier = await _context.Barriers.FindAsync(id);
            if (barrier == null)
            {
                return NotFound();
            }

            _context.Barriers.Remove(barrier);
            await _context.SaveChangesAsync();

            return _mapper.Map<BarrierResponse>(barrier);
        }

        private bool BarrierExists(int id)
        {
            return _context.Barriers.Any(e => e.BarrierId == id);
        }
    }
}
