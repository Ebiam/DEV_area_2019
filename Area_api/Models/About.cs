using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Area_api.Models
{
    public class client_json
    {
        public string host { get; set; }
    }
    public class params_json
    {
        public string name { get; set; }
        public string type { get; set; }
    }
    public class action_reaction_json
    {
        public string name { get; set; }
        public string description { get; set; }
    }
    public class service_json
    {
        public string name { get; set; }
        public List<action_reaction_json> actions { get; set; }
        public List<action_reaction_json> reactions { get; set; }
    }
    /*public class services_json
    {
        public string name { get; set; }
        public List<widgets_json> services { get; set; }
    }*/

    public class server_json
    {
        public int current_time { get; set; }
        public List<service_json> services { get; set; }
    }
    public class About
    {
        public client_json client { get; set; }
        public server_json server { get; set; }
    }
}
