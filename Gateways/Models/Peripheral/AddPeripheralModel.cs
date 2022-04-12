using System;
using System.ComponentModel.DataAnnotations;

namespace Gateways
{
    public class AddPeripheralModel
    {
        [Required]
        public int Id { get; set; }
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
