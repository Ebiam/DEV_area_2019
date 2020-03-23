using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models
{
    public class User
    {
        /*public User()
        {
            this.Services = new HashSet<Service>();
        }*/

        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        //public virtual ICollection<Service> Services { get; set; }
        //public virtual ICollection<Area> Areas { get; set; }
    }
}
