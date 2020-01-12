using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FissaBissa.Models
{
    public class ReservationModel
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public ICollection<string> Animals { get; set; }
    }
}
