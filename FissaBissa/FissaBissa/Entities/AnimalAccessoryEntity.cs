namespace FissaBissa.Entities
{
    public class AnimalAccessoryEntity
    {
        public int AnimalId { get; set; }
        public int AccessoryId { get; set; }

        public virtual AnimalEntity Animal { get; set; }
        public virtual AccessoryEntity Accessory { get; set; }
    }
}
