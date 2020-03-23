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
    public class AreaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AreaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Areas);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var a = _context.Areas.Where(a => a.userId == id);
            if (a.Count() > 0)
                return Ok(a);
            return BadRequest(new List<int>());
        }

        public class AreaModel2
        {
            public string userId { get; set; }

            public string actionId { get; set; }
            public string actionParam { get; set; }

            public string reactionId { get; set; }
            public string reactionParam { get; set; }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]AreaModel2 model)
        {
            if (model == null || !ModelState.IsValid )
                return BadRequest();
            Area area = new Area() { userId = (model.userId == "") ? 0 : Int32.Parse(model.userId), actionId = (model.actionId == "") ? 0 : Int32.Parse(model.actionId), actionParam = model.actionParam, reactionId = (model.reactionId == "") ? 0 : Int32.Parse(model.reactionId), reactionParam = model.reactionParam };

            _context.Areas.Add(area);
            _context.SaveChanges();
            Trigger tri = new Trigger() { areaId = area.id, act_reactId = (model.actionId == "") ? 0 : Int32.Parse(model.actionId), IsAction = true, init = true};
            /*switch (model.actionId)
            {
                case 1:
                    {
                        break;
                    }
                default:
                    break;
            }*/
            _context.Triggers.Add(tri);
            //_context.Areas.Add(area);
            _context.SaveChanges();
            return Ok(area);
        }

        public class AreaIdModel
        {
            public string areaId { get; set; }
        }

        // PUT api/values/5
        [HttpDelete]
        public IActionResult Delete([FromBody]AreaIdModel model)
        {
            if (model == null)
                return BadRequest();
            var che = _context.Areas.Where(a => a.id == ((model.areaId == "0") ? 0 : Int32.Parse(model.areaId)));
            if (che.Count() <= 0)
                return BadRequest();
            var tri = _context.Triggers.Where(t => t.areaId == ((model.areaId == "0") ? 0 : Int32.Parse(model.areaId)));
            if (tri.Count() > 0)
            {
                foreach (var trigg in tri)
                {
                    _context.Remove(trigg);
                }
            }
            _context.Remove(che.First());
            _context.SaveChanges();
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
