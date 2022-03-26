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
    public class ArtistsController : ApiController
    {
        private EventScheduleAPIContext db = new EventScheduleAPIContext();

        // GET: api/Artists
        public IQueryable<Artist> GetArtists()
        {
            return db.Artists;
        }

        // GET: api/Artists/5
        [ResponseType(typeof(Artist))]
        public async Task<IHttpActionResult> GetArtist(Guid id)
        {
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        // PUT: api/Artists/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArtist(Guid id, Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artist.ID)
            {
                return BadRequest();
            }

            db.Entry(artist).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/Artists
        [ResponseType(typeof(Artist))]
        public async Task<IHttpActionResult> PostArtist(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            artist.ID = Guid.NewGuid();
            db.Artists.Add(artist);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArtistExists(artist.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = artist.ID }, artist);
        }

        // DELETE: api/Artists/5
        [ResponseType(typeof(Artist))]
        public async Task<IHttpActionResult> DeleteArtist(Guid id)
        {
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }

            db.Artists.Remove(artist);
            await db.SaveChangesAsync();

            return Ok(artist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArtistExists(Guid id)
        {
            return db.Artists.Count(e => e.ID == id) > 0;
        }
    }
}