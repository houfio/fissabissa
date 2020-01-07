using System;
using System.Collections.Generic;

namespace FissaBissa.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public virtual List<AnimalReservationModel> Animals { get; set; }
        public virtual List<AccessoryReservationModel> Accessories { get; set; }
    }
}
