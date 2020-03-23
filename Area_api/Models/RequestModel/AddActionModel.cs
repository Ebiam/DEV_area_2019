using System;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models.RequestModel
{
    public class AddActionModel
    {
        [Required]
        public int serviceId { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
    }
}
