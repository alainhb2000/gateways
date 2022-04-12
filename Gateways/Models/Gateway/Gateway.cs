using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gateways
{
    public class Gateway
    {
        [Key]
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public List<Peripheral> Peripherals { get; set; } = new List<Peripheral>();

    }
}
