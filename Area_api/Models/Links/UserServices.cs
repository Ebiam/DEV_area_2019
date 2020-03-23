using System;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models.Links
{
    public class UserServices
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        public int serviceId { get; set; }
    }
}
