namespace FissaBissa.Entities
{
    public class AccessoryReservationEntity
    {
        public int AccessoryId { get; set; }
        public int ReservationId { get; set; }

        public virtual AccessoryEntity Accessory { get; set; }
        public virtual ReservationEntity Reservation { get; set; }
    }
}
