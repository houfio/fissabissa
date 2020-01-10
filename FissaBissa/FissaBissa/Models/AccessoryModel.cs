using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class AccessoryModel
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public float Price { get; set; }
        [Required] public string Image { get; set; }

        public virtual List<AnimalAccessoryModel> Animals { get; set; }
        public virtual List<AccessoryReservationModel> Reservations { get; set; }
    }
}
