using System;
using System.ComponentModel.DataAnnotations;

namespace Gateways
{
    public class Peripheral
    {
        [Key]
        public int Id { get; set; }
        public string Vendor { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
        public bool IsOnline { get; set; }
    }
}
