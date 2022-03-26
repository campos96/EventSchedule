using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EventSchedule.API.Data
{
    public class EventScheduleAPIContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public EventScheduleAPIContext() : base("name=EventScheduleAPIContext")
        {
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        public System.Data.Entity.DbSet<EventSchedule.API.Models.Stage> Stages { get; set; }

        public System.Data.Entity.DbSet<EventSchedule.API.Models.Artist> Artists { get; set; }

        public System.Data.Entity.DbSet<EventSchedule.API.Models.Event> Events { get; set; }

        public System.Data.Entity.DbSet<EventSchedule.API.Models.Presentation> Presentations { get; set; }
    }
}
