using Magic_vila_API.Data;
using Magic_vila_API.Models;
using Magic_vila_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Magic_vila_API.Controllers
{
    [Route("api/VilaAPI")]
    [ApiController]
    public class magicVilaController : ControllerBase
    {
        [HttpGet]
        
        public IEnumerable<VilaDTO> GetMagicVila()
        {
            return VilaStore.VilaList;
        }

        [HttpGet("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<VilaDTO> GetMagicVila(int Id)
        {
            if(Id < 0)
            {
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
    }

}
