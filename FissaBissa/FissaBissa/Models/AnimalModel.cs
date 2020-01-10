using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FissaBissa.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }

        [Required, StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, Display(Name = "Type")] public int TypeId { get; set; }
        [Required, Range(0, 10000)] public float Price { get; set; }
        [Required, FileExtensions] public IFormFile Image { get; set; }
    }
}
