using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area_api.Data;
using Area_api.Models;
using Area_api.Models.Links;
using Microsoft.AspNetCore.Mvc;
using Area_api.Models.RequestModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Area_api.Controllers
{
    [Route("[controller]")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Service> Get()
        {
            return _context.Services.ToArray();
        }

        //[Route("[controller]/GetUser")]
        [HttpPost("getuser")]
        public IActionResult GetUser([FromBody]AddServiceModel model)
        {
            if (model == null)
                return Ok(_context.Services.ToArray());
            var usr = _context.Users.Where(u => u.username == model.name);
            if (usr == null || usr.Count() == 0)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            //return new string[] { "value1", "value2" };
            var user = usr.First();
            var suscribed = _context.Apis.Where(us => us.userId == user.id);
            List<int> res = new List<int>();

            if (suscribed.Count() > 0)
            {


                foreach (var s in suscribed)
                {
                    res.Add(s.serviceId);
                }
                return Ok(res);
            }

            return Ok(res);
            //var res = new List<string>();

            /*if (usr.First() == null || usr.First().Services == null || usr.First().Services.Count() == 0)
                return Ok(null);
            var usrServices = usr.First().Services.ToArray();
            foreach (var service in usrServices)
            {
                res.Add(service.name);
            }
            return Ok(res);*/
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]ServiceModel model)
        {
            if (model == null)
                return BadRequest(new { message = "Wrong parameters" });
            var usr = _context.Users.Where(u => u.username == model.username);
            if (usr == null || usr.Count() == 0)
            {
                return BadRequest(new { message = "User not found" });
            }
            var user = usr.First();
            var serv = _context.Services.Where(s => s.id == model.serviceid);
            var service = serv.First();
            if (serv == null || serv.Count() == 0 || service == null)
                return BadRequest(new { message = "Service not found, wrong Id" });
            var userservices = _context.UserServices.Where(us => us.userId == user.id);
            if (userservices.Count() > 0)
            {
                var check = userservices.Where(s => s.serviceId == model.serviceid);
                if (check.Count() > 0)
                    return BadRequest(new { message = "User already suscribed to service number " + model.serviceid });

            }
            UserServices add = new UserServices();
            add.serviceId = model.serviceid;
            add.userId = user.id;
            _context.Add(add);
            _context.SaveChanges();
            /*if (user.Services == null)
                user.Services = new ICollection<Service>();*/
            //user.Services.Add(service);
            //_context.Update(user);
            return Ok("User " + user.username + " sucessfully added to service number " + serv.First().id);
        }

        public class TokenModel
        {
            public int userId { get; set; }
            public int serviceId { get; set; }
        }

        public class returnLinkModel
        {
            public string link { get; set; }
            public string link2 { get; set; }
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody]TokenModel model)
        {
            if (model == null)
                return BadRequest();

            var check = _context.Services.Where(s => s.id == model.serviceId);

            if (check.Count() == 0)
                return BadRequest("Service not found with id " + model.serviceId.ToString());

            returnLinkModel ret = new returnLinkModel();

            ret.link = check.First().link;
            ret.link2 = check.First().link2;
            return Ok(ret);
        }

        /*// GET api/values/5
        [HttpGet]
        public IActionResult Get()
        {
            var res = _context.Services.ToList();
            if (res != null)
                return Ok(_context.Services.ToList());
            return BadRequest(new { message = "No services available" });
        }*/

        // POST api/values

        [HttpPost]
        public IActionResult Post([FromBody]AddServiceModel model)
        {
            if (model != null)
            {
                var check = _context.Services.Where(s => s.name == model.name);
                if (check.Count() > 0)
                    return BadRequest(new { message = "Service already exist" });
                Service a = new Service { name = model.name, link = model.link };
                _context.Services.Add(a);
                _context.SaveChanges();
                return Ok("Service Added " + model.name);
            }
            //return Ok("Service Added " + name);
            if (model != null && model.name != null)
                return BadRequest(new { message = "No name given or already taken {" + model.name + "}" });
            return BadRequest();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string name, [FromBody]string description)
        {
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
