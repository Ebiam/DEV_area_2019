using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area_api.Data;
using Area_api.Models.RequestModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Area_api.Controllers
{
    [Route("[controller]")]
    public class ReactionController : Controller
    {
        // GET: api/values
        private readonly ApplicationDbContext _context;

        public ReactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var actions = _context.Reactions;
            if (actions.Count() == 0)
                return Ok(new List<int>());
            return Ok(actions);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var actions = _context.Reactions.Where(a => a.serviceId == id);
            if (actions.Count() == 0)
                return Ok(new List<int>());
            return Ok(actions);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]AddActionModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var check = _context.Reactions.Where(s => s.name == model.name && s.serviceId == model.serviceId);
                if (check.Count() > 0)
                    return BadRequest(new { message = "Action already exist" });
                Area_api.Models.Reaction a = new Area_api.Models.Reaction { name = model.name, serviceId = model.serviceId, description = model.description };
                _context.Reactions.Add(a);
                _context.SaveChanges();
                return Ok("Reaction Added " + model.name);
            }
            //return Ok("Service Added " + name);
            if (model != null && model.name != null)
                return BadRequest(new { message = "No name given or already taken {" + model.name + "}" });
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]RmvActReactModel model)
        {
            List<Models.Reaction> todel = new List<Models.Reaction>();
            todel = _context.Reactions.Where(u => u.serviceId == model.serviceId && u.name == model.name).ToList();

            if (todel.Count() >= 1)
            {
                var a = new List<string>();
                foreach (var usr in todel)
                {
                    _context.Remove(usr);
                    a.Add(usr.name);
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
    }
}
