using System;
using System.ComponentModel.DataAnnotations;

namespace Backend_schronisko.Models
{
    public class WniosekOddania
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImieINazwiskoOddajacego { get; set; }

        [Required]
        [Phone]
        public string Telefon { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ImieZwierzaka { get; set; }

        [Required]
        public string Gatunek { get; set; } // Pies lub Kot

        [Required]
        public string PowodOddania { get; set; }

        public DateTime DataZlozenia { get; set; } = DateTime.Now;
    }
}