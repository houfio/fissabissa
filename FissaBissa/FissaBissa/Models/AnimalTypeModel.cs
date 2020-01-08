using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class AnimalTypeModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
