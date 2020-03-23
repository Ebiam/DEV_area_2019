using System;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models
{
    public class Area
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }

        public int actionId { get; set; }
        public string actionParam { get; set; }

        public int reactionId { get; set; }
        public string reactionParam { get; set; }
    }
}
