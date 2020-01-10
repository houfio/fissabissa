using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Entities
{
    public class AnimalEntity
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public int TypeId { get; set; }
        [Required] public float Price { get; set; }
        [Required] public string Image { get; set; }

        public virtual AnimalTypeEntity Type { get; set; }
        public virtual List<AnimalAccessoryEntity> Accessories { get; set; }
        public virtual List<AnimalReservationEntity> Reservations { get; set; }
    }
}
