﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static JoinMyCarTrip.Data.DataConstants;

namespace JoinMyCarTrip.Data.Entities
{
    public class Message
    {

        [Key]
        [MaxLength(GuidMaxLength)]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey(nameof(Author))]
        [Required]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }


        [ForeignKey(nameof(Trip))]
        [Required]
        public string TripId { get; set; }
        public Trip Trip { get; set; } 


    }
}
