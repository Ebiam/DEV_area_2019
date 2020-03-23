using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models
{
    public class Service
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string link2 { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        //public virtual ICollection<Action> Actions { get; set; }
        //public virtual ICollection<Reaction> Reactions { get; set; }
    }
}
