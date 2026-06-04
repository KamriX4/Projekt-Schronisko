using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_schronisko.Models
{
    public class WniosekAdopcyjny
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImieINazwisko { get; set; }

        [Required]
        [Phone]
        public string Telefon { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int ZwierzeId { get; set; }

        [Required]
        public string Uzasadnienie { get; set; }

        public DateTime DataZlozenia { get; set; } = DateTime.Now;

        // Opcjonalnie: relacja do zwierzęcia, żeby móc pobrać jego dane
        [ForeignKey("ZwierzeId")]
        public Zwierze? Zwierze { get; set; }
    }
}