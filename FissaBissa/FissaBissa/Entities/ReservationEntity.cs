using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Entities
{
    public class ReservationEntity
    {
        public int Id { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public virtual List<AnimalReservationEntity> Animals { get; set; }
        public virtual List<AccessoryReservationEntity> Accessories { get; set; }
    }
}
