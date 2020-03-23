using System;

namespace Area_api.Models.RequestModel
{
    public class UserApiModel2
    {
        public /*int*/string userId { get; set; }
        public /*int*/string serviceId { get; set; }

        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string username { get; set; }
        public /*int*/string accountId { get; set; }
    }
}
