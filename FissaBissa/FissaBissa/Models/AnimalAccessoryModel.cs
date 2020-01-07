namespace FissaBissa.Models
{
    public class AnimalAccessoryModel
    {
        public int AnimalId { get; set; }
        public int AccessoryId { get; set; }

        public virtual AnimalModel Animal { get; set; }
        public virtual AccessoryModel Accessory { get; set; }
    }
}
