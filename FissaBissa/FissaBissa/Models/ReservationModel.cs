using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public virtual List<AnimalReservationModel> Animals { get; set; }
        public virtual List<AccessoryReservationModel> Accessories { get; set; }
    }
}
