using System;
using System.ComponentModel.DataAnnotations;

namespace Area_api.Models.Apis
{
    public class ApiToken
    {
        [Key]
        public int id { get; set; }

        public int userId { get; set; }
        public int serviceId { get; set; }

        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string username { get; set; }
        public int accountId { get; set; }
    }
}
