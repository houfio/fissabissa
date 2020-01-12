using System;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Entities
{
    public class AnimalTypeEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
