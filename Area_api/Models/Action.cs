using System;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models
{
    public class Action
    {
        [Key]
        public int id { get; set; }
        public int serviceId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
