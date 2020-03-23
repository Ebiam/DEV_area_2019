using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area_api.Data;
using Area_api.Models;
using Area_api.Models.RequestModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Area_api.Controllers
{
    [Route("[controller]")]
    public class ActionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var actions = _context.Actions;
            if (actions.Count() == 0)
                return Ok(new List<int>());
            return Ok(actions);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var actions = _context.Actions.Where(a => a.serviceId == id);
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
                var check = _context.Actions.Where(s => s.name == model.name && s.serviceId == model.serviceId);
                if (check.Count() > 0)
                    return BadRequest(new { message = "Action already exist" });
                Area_api.Models.Action a = new Area_api.Models.Action { name = model.name, serviceId = model.serviceId, description = model.description };
                _context.Actions.Add(a);
                _context.SaveChanges();
                return Ok("Action Added " + model.name);
            }
            //return Ok("Service Added " + name);
            if (model != null && model.name != null)
                return BadRequest(new { message = "No name given or already taken {" + model.name + "}" });
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]RmvActReactModel model)
        {
            List<Models.Action> todel = new List<Models.Action>();
            todel = _context.Actions.Where(u => u.serviceId == model.serviceId && u.name == model.name).ToList();

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
