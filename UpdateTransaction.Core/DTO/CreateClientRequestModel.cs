using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTransaction.Core.DTO
{
    public class CreateClientRequestModel
    {
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string ClientAddress { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
