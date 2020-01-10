using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Entities
{
    public class AnimalTypeEntity
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
    }
}
