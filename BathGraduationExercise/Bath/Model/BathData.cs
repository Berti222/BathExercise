using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bath
{
    public enum GuestSate { CheckIn, CheckOut }
    public class BathData
    {
        public int Id { get; set; }
        public string GuestId { get; set; }

        [ForeignKey("BathArea")]
        public int BathAreaId { get; set; }
        public BathArea BathArea { get; set; }

        public GuestSate GuestSate { get; set; }

        public DateTime CheckingTime { get; set; }
    }
}
