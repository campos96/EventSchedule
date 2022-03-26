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
    public class StagesController : ApiController
    {
        private EventScheduleAPIContext db = new EventScheduleAPIContext();

        // GET: api/Stages
        public IQueryable<Stage> GetStages()
        {
            return db.Stages;
        }

        // GET: api/Stages/5
        [ResponseType(typeof(Stage))]
        public async Task<IHttpActionResult> GetStage(Guid id)
        {
            Stage stage = await db.Stages.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }

            return Ok(stage);
        }

        // PUT: api/Stages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStage(Guid id, Stage stage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stage.ID)
            {
                return BadRequest();
            }

            db.Entry(stage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StageExists(id))
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

        // POST: api/Stages
        [ResponseType(typeof(Stage))]
        public async Task<IHttpActionResult> PostStage(Stage stage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            stage.ID = Guid.NewGuid();
            db.Stages.Add(stage);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StageExists(stage.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = stage.ID }, stage);
        }

        // DELETE: api/Stages/5
        [ResponseType(typeof(Stage))]
        public async Task<IHttpActionResult> DeleteStage(Guid id)
        {
            Stage stage = await db.Stages.FindAsync(id);
            if (stage == null)
            {
                return NotFound();
            }

            db.Stages.Remove(stage);
            await db.SaveChangesAsync();

            return Ok(stage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StageExists(Guid id)
        {
            return db.Stages.Count(e => e.ID == id) > 0;
        }
    }
}