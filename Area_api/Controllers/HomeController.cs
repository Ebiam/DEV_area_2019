using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Area_api.Data;
using Area_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Area_api.Controllers
{
    //[Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/values
        
        [HttpGet("about.json")]
        public ContentResult Get()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            About about = new About();
            

            server_json serv = new server_json();
            var dateTime = DateTime.Now;
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalSeconds;
            serv.current_time = Convert.ToInt32(unixDateTime);
            serv.services = new List<service_json>();//new List<services_json>();
            about.server = serv;

            client_json cli = new client_json();
            //cli.host = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.IsIPv6LinkLocal == true)
                cli.host = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();//.ToString();
            else
                cli.host = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //about.client = cli;

            //string externalIP = "";
            var request = (HttpWebRequest)WebRequest.Create("http://icanhazip.com.ipaddress.com/");
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            cli.host = new WebClient().DownloadString("http://icanhazip.com");
            cli.host = cli.host.Remove(cli.host.Length - 1);
            about.client = cli;
            //string myHost = System.Net.Dns.GetHostName();
            //cli.host = System.Net.Dns.GetHostEntry(myHost).AddressList[0].ToString();//Request.ServerVariables["LOCAL_ADDR"];

            /*var client = new client_json();
            var httpConnectionFeature = httpContext.Features.Get<IHttpConnectionFeature>();
            var localIpAddress = httpConnectionFeature?.LocalIpAddress;

            client.host = localIpAddress;*/

            var services = _context.Services;
            foreach (var s in services)
            {
                service_json ser = new service_json();

                ser.name = s.name;

                ser.actions = new List<action_reaction_json>();
                var li = _context.Actions.Where(a => a.serviceId == s.id);
                foreach (var act in li)
                {
                    action_reaction_json action = new action_reaction_json();
                    action.name = act.name;
                    action.description = act.description;
                    ser.actions.Add(action);
                }

                ser.reactions = new List<action_reaction_json>();
                var rl = _context.Reactions.Where(a => a.serviceId == s.id);
                foreach (var rea in rl)
                {
                    action_reaction_json reaction = new action_reaction_json();
                    reaction.name = rea.name;
                    reaction.description = rea.description;
                    ser.reactions.Add(reaction);
                }

                serv.services.Add(ser);
            }


            var modelJson = JsonSerializer.Serialize(about, options);
            return Content(modelJson);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
