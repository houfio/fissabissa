using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class ReservationModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ICollection<Guid> Animals { get; set; } = new List<Guid>();

        public ICollection<Guid> Accessories { get; set; } = new List<Guid>();

        [Required, Display(Name = "Name"), StringLength(100, MinimumLength = 6)]
        public string FullName { get; set; }

        [Required, StringLength(100, MinimumLength = 6)]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string Price { get; set; }
    }
}
