using System;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class ReservationModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
