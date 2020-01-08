using System.Collections.Generic;

namespace FissaBissa.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }

        public virtual AnimalTypeModel Type { get; set; }
        public virtual List<AnimalAccessoryModel> Accessories { get; set; }
        public virtual List<AnimalReservationModel> Reservations { get; set; }
    }
}
