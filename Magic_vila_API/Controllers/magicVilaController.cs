using Magic_vila_API.Data;
using Magic_vila_API.Models;
using Magic_vila_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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
    }

  

}
