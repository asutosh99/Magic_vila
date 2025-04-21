using Magic_vila_API.Data;
//using Magic_vila_API.Logging;
using Magic_vila_API.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Magic_vila_API.Controllers
{
    [Route("api/VilaAPI")]
    [ApiController]
    public class magicVilaController : ControllerBase
    {
        private readonly ILogger<magicVilaController> _logger;

        public magicVilaController(ILogger<magicVilaController> logger)
        {
            _logger = logger;
        }
        //This is for custom log implementation 
        /*private readonly ILogging _logger;
        public magicVilaController(ILogging logger) {
            _logger = logger;
        }*/
        [HttpGet]
        
        public IEnumerable<VilaDTO> GetMagicVila()
        {
            _logger.LogInformation("get all Vilas");
           // _logger.Log("get all Villas", "Info");
            return VilaStore.VilaList;
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
            var Vila= VilaStore.VilaList.FirstOrDefault(u => u.Id == Id);
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
            if(VilaStore.VilaList.FirstOrDefault(u => u.Name.ToLower() == VilaDTO.Name)!=null) {
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
            VilaDTO.Id = VilaStore.VilaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            Console.WriteLine(VilaDTO);
            if (VilaDTO.Id > 0)
            {
                VilaStore.VilaList.Add(VilaDTO);
            }
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

            var Vila=VilaStore.VilaList.FirstOrDefault(u => u.Id == id);
            if(Vila == null)
            {
                return NotFound();
            }
            VilaStore.VilaList.Remove(Vila);
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
            var vila=VilaStore.VilaList.FirstOrDefault(v => v.Id == id);
            vila.Name = VilaDTO.Name;
            vila.sqft = VilaDTO.sqft;
            vila.occupancy = VilaDTO.occupancy;
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
            var vila= VilaStore.VilaList.FirstOrDefault(u=>u.Id == id);
            if(vila == null)
            {
                return BadRequest();
            }
            PatchObj.ApplyTo(vila,ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }

}
