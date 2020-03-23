using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Area_api.Data;
using Area_api.Models;
using Area_api.Models.RequestModel;
using Area_api.Models.Apis;
//using System.ServiceModel.DomainServices.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Area_api.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.Users.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var a = _context.Users.Where(u => u.username == id.ToString());
            if (a.Count() > 0)
                return BadRequest(new { message = "No name given or already taken "});
            _context.Add(new User { username = id.ToString(), password = "pass" });
            _context.SaveChanges();
            return Ok();
        }

        // POST api/values
        //[Route("[controller]")]
        [HttpPost]
        public IActionResult Post([FromBody]UserRegistrationModel model)
        {
            if (model == null)
                return BadRequest(new { message = "No model given" });
            if (model.username == null)
                return BadRequest(new { message = "No name given" });
            var a = _context.Users.Where(u => u.username == model.username);
            if (a.Count() > 0)
                return BadRequest(new { message = "Name already taken " });
            var user = new User { username = model.username, password = model.password };
            _context.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody]AddServiceModel model)
        {
            List<User> todel = new List<User>();
            todel = _context.Users.Where(u => u.username == model.name).ToList();
          
            if (todel.Count() >= 1) {
                var a = new List<string>();
                foreach (var usr in todel)
                {
                    _context.Remove(usr);
                   a.Add(usr.username);
                }
                string b = "";
                foreach (var c in a)
                {
                    b += b + ' ' + c;
                }
                //_context.Remove(todel);
                _context.SaveChanges();
                return Ok("ok " + b + " deleted");
            }
            return BadRequest("ko");
        }

        [HttpPost("apiregister")]
        public IActionResult ApiRegister([FromBody]UserApiModel2 model)
        {
            if (model == null)
                return BadRequest("invalid body");
            var verif = _context.Apis.Where(a => a.userId == Int32.Parse(model.userId) && a.serviceId == Int32.Parse(model.serviceId));

            /*if (verif.Count() > 0)
                return Ok();*/

            ApiToken a = new ApiToken() { accessToken = model.accessToken, accountId = ((model.accountId == "") ? 0 : Int32.Parse(model.accountId)), refreshToken = model.refreshToken, userId = ((model.userId == "") ? 0 : Int32.Parse(model.userId)), serviceId = ((model.serviceId == "") ? 0 : Int32.Parse(model.serviceId)), username = model.username };
            _context.Add(a);
            _context.SaveChanges();
            return Ok();
        }

        public class ApiIdModel
        {
            public int id { get; set; }
        }

        [HttpDelete("apiregister")]
        public IActionResult ApiDelete([FromBody]ApiIdModel model)
        {
            if (model == null)
                return BadRequest("invalid body");
            var verif = _context.Apis.Where(a => a.id == model.id);
            if (verif.Count() == 0)
                return BadRequest();

            _context.Remove(verif.First());
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("tokens")]
        public IActionResult GetTokens([FromBody] UserIdModel model)
        {
            var verif = _context.Apis.Where(a => a.userId == model.userId);

            if (verif.Count() > 0)
                return Ok(verif);
            return Ok(new List<int>());

            /*ApiToken a = new ApiToken() { accessToken = model.accessToken, accountId = model.accountId, refreshToken = model.refreshToken, userId = model.userId, serviceId = model.serviceId, username = model.username };
            _context.Add(a);
            _context.SaveChanges();
            return Ok();*/
        }

        

    }
}
