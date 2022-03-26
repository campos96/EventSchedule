using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventSchedule.API.Models
{
    public class Event
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<Stage> Stages { get; set; }
    }
}