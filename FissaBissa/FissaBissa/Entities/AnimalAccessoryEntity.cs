using System;

namespace FissaBissa.Entities
{
    public class AnimalAccessoryEntity
    {
        public Guid AnimalId { get; set; }
        public Guid AccessoryId { get; set; }

        public virtual AnimalEntity Animal { get; set; }
        public virtual AccessoryEntity Accessory { get; set; }
    }
}
