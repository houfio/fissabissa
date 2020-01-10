namespace FissaBissa.Entities
{
    public class AnimalReservationEntity
    {
        public int AnimalId { get; set; }
        public int ReservationId { get; set; }

        public virtual AnimalEntity Animal { get; set; }
        public virtual ReservationEntity Reservation { get; set; }
    }
}
