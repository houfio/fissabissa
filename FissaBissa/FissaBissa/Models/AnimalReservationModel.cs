namespace FissaBissa.Models
{
    public class AnimalReservationModel
    {
        public int AnimalId { get; set; }
        public int ReservationId { get; set; }

        public virtual AnimalModel Animal { get; set; }
        public virtual ReservationModel Reservation { get; set; }
    }
}
