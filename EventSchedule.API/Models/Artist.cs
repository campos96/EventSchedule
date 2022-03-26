using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventSchedule.API.Models
{
    public class Artist
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Photo { get; set; }

    }
}