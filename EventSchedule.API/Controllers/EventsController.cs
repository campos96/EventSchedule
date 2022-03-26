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
    public class EventsController : ApiController
    {
        private EventScheduleAPIContext db = new EventScheduleAPIContext();

        // GET: api/Events
        public IQueryable<Event> GetEvents()
        {
            return db.Events;
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(Guid id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        [Route("api/Events/{id}/Details")]
        public async Task<IHttpActionResult> GetEventDetails(Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var _event = await db.Events.FindAsync(id);

            if (_event == null)
            {
                return NotFound();
            }

            _event.Stages = await db.Stages
                .Include(s => s.Presentations)
                .Include(s => s.Presentations.Select(p => p.Artist))
                .Where(s => s.EventID == _event.ID)
                .OrderBy(s => s.Order)
                .ToListAsync();

            return Ok(_event);
        }

        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent(Guid id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.ID)
            {
                return BadRequest();
            }

            db.Entry(@event).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            @event.ID = Guid.NewGuid();
            db.Events.Add(@event);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventExists(@event.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = @event.ID }, @event);
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(Guid id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            db.Events.Remove(@event);
            await db.SaveChangesAsync();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(Guid id)
        {
            return db.Events.Count(e => e.ID == id) > 0;
        }
    }
}