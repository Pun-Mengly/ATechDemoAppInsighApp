using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ATechDemoAppInsigh.Models;

namespace ATechDemoAppInsigh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentsContext _context;
        private readonly ILogger<StudentsController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentsController(StudentsContext context, ILogger<StudentsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tblstudent>>> GetTblstudents()
        {
            if (_context.Tblstudents == null)
            {
                return NotFound(new MessageHandler(false, RequestId(), null));
            }
            return Ok(new MessageHandler(true, RequestId(), await _context.Tblstudents.ToListAsync()));
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tblstudent>> GetTblstudent(int id)
        {
            if (_context.Tblstudents == null)
            {
                return NotFound(new MessageHandler(false, RequestId(), null));
            }
            var tblstudent = await _context.Tblstudents.FindAsync(id);

            if (tblstudent == null)
            {
                return NotFound(new MessageHandler(false, RequestId(), null));
            }

            return Ok(new MessageHandler(true, RequestId(), tblstudent));
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblstudent(int id, Tblstudent tblstudent)
        {
            if (id != tblstudent.Id)
            {
                return BadRequest();
            }

            _context.Entry(tblstudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblstudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tblstudent>> PostTblstudent(Tblstudent tblstudent)
        {
            if (_context.Tblstudents == null)
            {
                return Problem("Entity set 'StudentsContext.Tblstudents'  is null.");
            }
            _context.Tblstudents.Add(tblstudent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TblstudentExists(tblstudent.Id))
                {
                    _logger.LogError($"ID= {tblstudent.Id} is Conflict...");
                    return Conflict(new MessageHandler(false, RequestId(), null));
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTblstudent", new { id = tblstudent.Id }, tblstudent);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblstudent(int id)
        {
            if (_context.Tblstudents == null)
            {
                return NotFound();
            }
            var tblstudent = await _context.Tblstudents.FindAsync(id);
            if (tblstudent == null)
            {
                return NotFound();
            }

            _context.Tblstudents.Remove(tblstudent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TblstudentExists(int id)
        {
            return (_context.Tblstudents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private string RequestId()
        {
             return _httpContextAccessor.HttpContext!.TraceIdentifier;
        }
}
    public record MessageHandler(bool Status,string RequestId,object Data);
}
