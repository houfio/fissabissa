using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Entities
{
    public class AccessoryEntity
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public float Price { get; set; }
        [Required] public string Image { get; set; }

        public virtual List<AnimalAccessoryEntity> Animals { get; set; }
        public virtual List<AccessoryReservationEntity> Reservations { get; set; }
    }
}
