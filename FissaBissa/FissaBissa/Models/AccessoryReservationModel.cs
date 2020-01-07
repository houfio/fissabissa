namespace FissaBissa.Models
{
    public class AccessoryReservationModel
    {
        public int AccessoryId { get; set; }
        public int ReservationId { get; set; }

        public virtual AccessoryModel Accessory { get; set; }
        public virtual ReservationModel Reservation { get; set; }
    }
}
