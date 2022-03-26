using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EventSchedule.API.Data;
using EventSchedule.API.Models;

namespace EventSchedule.API.Controllers
{
    public class PresentationsController : ApiController
    {
        private EventScheduleAPIContext db = new EventScheduleAPIContext();

        // GET: api/Presentations
        public IQueryable<Presentation> GetPresentations()
        {
            var presentations = db.Presentations
                .Include(p => p.Artist)
                .Include(p => p.Stage)
                .ToList();
            return db.Presentations;
        }

        // GET: api/Presentations/5
        [ResponseType(typeof(Presentation))]
        public async Task<IHttpActionResult> GetPresentation(Guid id)
        {
            Presentation presentation = await db.Presentations.FindAsync(id);
            if (presentation == null)
            {
                return NotFound();
            }

            return Ok(presentation);
        }

        // PUT: api/Presentations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPresentation(Guid id, Presentation presentation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != presentation.ID)
            {
                return BadRequest();
            }

            db.Entry(presentation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Presentations
        [ResponseType(typeof(Presentation))]
        public async Task<IHttpActionResult> PostPresentation(Presentation presentation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            presentation.ID = Guid.NewGuid();
            db.Presentations.Add(presentation);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PresentationExists(presentation.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = presentation.ID }, presentation);
        }

        // DELETE: api/Presentations/5
        [ResponseType(typeof(Presentation))]
        public async Task<IHttpActionResult> DeletePresentation(Guid id)
        {
            Presentation presentation = await db.Presentations.FindAsync(id);
            if (presentation == null)
            {
                return NotFound();
            }

            db.Presentations.Remove(presentation);
            await db.SaveChangesAsync();

            return Ok(presentation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PresentationExists(Guid id)
        {
            return db.Presentations.Count(e => e.ID == id) > 0;
        }
    }
}