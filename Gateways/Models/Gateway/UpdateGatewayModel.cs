using System.ComponentModel.DataAnnotations;

namespace Gateways
{
    public class UpdateGatewayModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$")]
        public string IP { get; set; }
    }

}
