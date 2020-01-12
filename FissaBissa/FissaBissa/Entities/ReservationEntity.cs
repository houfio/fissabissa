using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FissaBissa.Models;
using FissaBissa.Utilities;

namespace FissaBissa.Entities
{
    public class ReservationEntity : ITransformer<ReservationModel>
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Email { get; set; }
        public string Telephone { get; set; }

        public virtual ICollection<AnimalReservationEntity> Animals { get; set; }
        public virtual ICollection<AccessoryReservationEntity> Accessories { get; set; }

        public ReservationModel Transform()
        {
            return new ReservationModel();
        }

        public void Copy(ReservationModel data, bool create)
        {
            Date = data.Date;
        }
    }
}
