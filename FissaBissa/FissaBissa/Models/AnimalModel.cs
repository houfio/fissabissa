using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class AnimalModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Image { get; set; }

        public virtual AnimalTypeModel Type { get; set; }
        public virtual List<AnimalAccessoryModel> Accessories { get; set; }
        public virtual List<AnimalReservationModel> Reservations { get; set; }
    }
}
