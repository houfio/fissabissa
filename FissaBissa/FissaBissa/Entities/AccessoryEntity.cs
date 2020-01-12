using System;
using FissaBissa.Models;
using FissaBissa.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Entities
{
    public class AccessoryEntity : ITransformer<AccessoryModel>
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Image { get; set; }

        public virtual ICollection<AnimalAccessoryEntity> Animals { get; set; }
        public virtual ICollection<AccessoryReservationEntity> Reservations { get; set; }

        public AccessoryModel Transform()
        {
            return new AccessoryModel
            {
                Id = Id,
                Name = Name,
                Price = Price
            };
        }

        public void Copy(AccessoryModel data, bool create)
        {
            Name = data.Name;
            Price = data.Price;

            if (create)
            {
                Animals = new List<AnimalAccessoryEntity>();
                Reservations = new List<AccessoryReservationEntity>();
            }
        }
    }
}
