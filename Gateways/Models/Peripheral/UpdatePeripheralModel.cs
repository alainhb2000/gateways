using System;
using System.ComponentModel.DataAnnotations;

namespace Gateways
{
    public class UpdatePeripheralModel
    {
        [Required]
        [MinLength(2)]
        public string Vendor { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreationDate { get; set; }
        public bool? IsOnline { get; set; }
    }
}
