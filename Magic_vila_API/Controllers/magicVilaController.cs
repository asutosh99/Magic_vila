using Magic_vila_API.Data;
using Magic_vila_API.Models;

//using Magic_vila_API.Logging;
using Magic_vila_API.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Magic_vila_API.Controllers
{
    [Route("api/VilaAPI")]
    [ApiController]
    public class magicVilaController : ControllerBase
    {
        private readonly ILogger<magicVilaController> _logger;

        /*public magicVilaController(ILogger<magicVilaController> logger)
        {
            _logger = logger;
        }*/

        private readonly ApplicationDbContext _db;

        public magicVilaController(ILogger<magicVilaController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        //This is for custom log implementation 
        /*private readonly ILogging _logger;
        public magicVilaController(ILogging logger) {
            _logger = logger;
        }*/
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VilaDTO>> GetMagicVila()
        {
            _logger.LogInformation("get all Vilas");
           // _logger.Log("get all Villas", "Info");
            return Ok(_db.Vilas.ToList());
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VilaDTO> GetMagicVila(int Id)
        {
            if(Id == 0)
            {
                _logger.LogError("getting by Id error Id is Invalid");
             //   _logger.Log("getting by Id error Id is Invalid", "Error");
                return BadRequest();
            }
            var Vila= _db.Vilas.FirstOrDefault(u => u.Id == Id);
            if(Vila == null)
            {
                return NotFound();
            }
            return Ok(Vila);
        }

        [HttpPost]

        public ActionResult<VilaDTO> CreateVila([FromBody] VilaDTO VilaDTO)
        {
            /*if(ModelState.IsValid)
            {

            }*/
            if(_db.Vilas.FirstOrDefault(u => u.Name.ToLower() == VilaDTO.Name)!=null) {
                //return BadRequest("Name already exist");
                 ModelState.AddModelError("customError","Name duplication");
                return BadRequest(ModelState);
            }
            if (VilaDTO == null)
            {
                return BadRequest(VilaDTO);
            }
            if (VilaDTO.Id > 0)
            {
               return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Console.WriteLine(VilaDTO);
            Vila model=new Vila()
            {
                Amenity = VilaDTO.Amenity,
                Name    = VilaDTO.Name,
                Details = VilaDTO.Details,  
                Rate = VilaDTO.Rate,    
                Sqft = VilaDTO.Sqft,
                Occupancy = VilaDTO.Occupancy,
                ImageUrl    = VilaDTO.ImageUrl,
                Id  = VilaDTO.Id,
            };
            
                _db.Vilas.Add(model);
            _db.SaveChanges();
            
            return Ok(VilaDTO);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteVila([FromBody] int  id)
        {

            if(id < 0)
            {
                return BadRequest();
            }

            var Vila=_db.Vilas.FirstOrDefault(u => u.Id == id);
            if(Vila == null)
            {
                return NotFound();
            }
            _db.Vilas.Remove(Vila);
            _db.SaveChanges();
            return NoContent();
        }

       
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVila(int id,[FromBody] VilaDTO VilaDTO)
        {
            if (VilaDTO.Id != id || VilaDTO == null)
            {
                return BadRequest();
            }
            var vila=_db.Vilas.FirstOrDefault(v => v.Id == id);
            /*       vila.Name = VilaDTO.Name;
                   vila.Sqft = VilaDTO.Sqft;
                   vila.Occupancy = VilaDTO.Occupancy;*/
            Vila model = new Vila()
            {
                Name = VilaDTO.Name,
                Amenity = VilaDTO.Amenity,
                Id = VilaDTO.Id,
                Occupancy = VilaDTO.Occupancy,
                Rate = VilaDTO.Rate,
                Sqft = VilaDTO.Sqft,
            };

            _db.Update(model);
            _db.SaveChanges();

            return NoContent();
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePatchVila(int id,JsonPatchDocument<VilaDTO> PatchObj)
        {
            if(id== null||id < 0  ||PatchObj== null)
            {
                return BadRequest();
            }
            var vila= _db.Vilas.AsNoTracking().FirstOrDefault(u=>u.Id == id);
            if(vila == null)
            {
                return BadRequest();
            }

            VilaDTO vilaDTO = new()
            {
                Name = vila.Name,
                Details = vila.Details,
                Amenity = vila.Amenity,
                Rate = vila.Rate,
                Sqft = vila.Sqft,
                Occupancy = vila.Occupancy,
                Id = vila.Id,
                ImageUrl = vila.ImageUrl
            };

            PatchObj.ApplyTo(vilaDTO,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Vila model = new()
            {
                Name = vilaDTO.Name,
                ImageUrl = vilaDTO.ImageUrl,
                Id = vilaDTO.Id,
                Occupancy = vilaDTO.Occupancy,
                Rate = vilaDTO.Rate,
                Sqft = vilaDTO.Sqft,
                Amenity=vilaDTO.Amenity
            }; 
            _db.Vilas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }
    }

}
