using System;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models
{
    public class Trigger
    {
        [Key]
        public int id { get; set; }
        public bool init { get; set; }
        public int areaId { get; set; }
        public bool IsAction { get; set; }
        public int act_reactId { get; set; }
        public int Ivalue { get; set; }
        public string Svalue { get; set; }
    }
}
