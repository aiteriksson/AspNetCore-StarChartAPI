using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext  context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if(celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Name == name);
            if (celestialObjects.Count() == 0)
            {
                return NotFound();
            }

            foreach (var o in celestialObjects)
            {
                o.Satellites = _context.CelestialObjects.Where(y => y.OrbitedObjectId == o.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            foreach (var o in celestialObjects)
            {
                o.Satellites = _context.CelestialObjects.Where(y => y.OrbitedObjectId == o.Id).ToList();
            }

            return Ok(celestialObjects);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            var entityEntry = _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = entityEntry.Entity.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject celestialObject)
        {
            var oldCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (oldCelestialObject == null)
            {
                return NotFound();
            }

            oldCelestialObject.Name = celestialObject.Name;
            oldCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            oldCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;

            _context.Update(oldCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var oldCelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (oldCelestialObject == null)
            {
                return NotFound();
            }

            oldCelestialObject.Name = name;
            _context.Update(oldCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Id == id);

            var orbitedObjects = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id);

            var allObjects = orbitedObjects.Union(celestialObjects);

            if (allObjects.Count() == 0)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(allObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
