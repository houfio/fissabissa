using System;

namespace FissaBissa.Entities
{
    public class AccessoryReservationEntity
    {
        public Guid AccessoryId { get; set; }
        public Guid ReservationId { get; set; }

        public virtual AccessoryEntity Accessory { get; set; }
        public virtual ReservationEntity Reservation { get; set; }
    }
}
