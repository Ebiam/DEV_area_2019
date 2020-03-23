using System;
namespace Area_api.Models.RequestModel
{
    public class AreaModel
    {

        public int userId { get; set; }

        public int actionId { get; set; }
        public string actionParam { get; set; }

        public int reactionId { get; set; }
        public string reactionParam { get; set; }

        /*public int UserId { get; set; }
        public int ActionId { get; set; }
        public string[] ActionParams { get; set; }
        public int ReactionId { get; set; }
        public string[] RectionParams { get; set; }*/
    }
}
