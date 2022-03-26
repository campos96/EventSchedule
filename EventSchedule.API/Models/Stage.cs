using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventSchedule.API.Models
{
    public class Stage
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public Guid EventID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Image { get; set; }

        public string Location { get; set; }

        [ForeignKey("EventID")]
        public Event Event { get; set; }

        [Required]
        public int Order { get; set; } 

        public ICollection<Presentation> Presentations { get; set; }
    }
}