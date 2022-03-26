using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EventSchedule.API.Models
{
    public class Presentation
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public Guid StageID { get; set; }

        [Required]
        public Guid ArtistID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("StageID")]
        public Stage Stage { get; set; }

        [ForeignKey("ArtistID")]
        public Artist Artist { get; set; }
    }
}