using System.Collections.Generic;

namespace FissaBissa.Models
{
    public class AccessoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }

        public virtual List<AnimalAccessoryModel> Animals { get; set; }
        public virtual List<AccessoryReservationModel> Reservations { get; set; }
    }
}
