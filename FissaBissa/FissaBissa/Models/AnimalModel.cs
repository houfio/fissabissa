using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FissaBissa.Models
{
    public class AnimalModel
    {
        public Guid Id { get; set; }

        [Required, StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, Display(Name = "Type")]
        public Guid TypeId { get; set; }

        [Required, Range(0, 10000), DataType(DataType.Currency)]
        public float Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public ICollection<Guid> Accessories { get; set; }
    }
}
