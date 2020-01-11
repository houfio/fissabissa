using System;

namespace FissaBissa.Entities
{
    public class AnimalReservationEntity
    {
        public Guid AnimalId { get; set; }
        public Guid ReservationId { get; set; }

        public virtual AnimalEntity Animal { get; set; }
        public virtual ReservationEntity Reservation { get; set; }
    }
}
