﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FissaBissa.Models;
using FissaBissa.Utilities;

namespace FissaBissa.Entities
{
    public class AnimalEntity : ITransformer<AnimalModel>
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public string Image { get; set; }

        public virtual AnimalTypeEntity Type { get; set; }
        public virtual List<AnimalAccessoryEntity> Accessories { get; set; }
        public virtual List<AnimalReservationEntity> Reservations { get; set; }

        public AnimalModel Transform()
        {
            return new AnimalModel
            {
                Id = Id,
                Name = Name,
                TypeId = TypeId,
                Price = Price
            };
        }

        public void Copy(AnimalModel data, bool create)
        {
            Name = data.Name;
            TypeId = data.TypeId;
            Price = data.Price;
        }
    }
}
