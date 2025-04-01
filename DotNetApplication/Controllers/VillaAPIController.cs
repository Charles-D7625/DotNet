using DotNetApplication.Models;
using DotNetApplication.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using DotNetApplication.Data;
using DotNetApplication.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace DotNetApplication.Controllers;

[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    private readonly ILogging _logger;

    public VillaAPIController(ILogging logger)
    {
        this._logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logger.Log("GetVillas", "");
        return Ok(VillaStore.villasList);
    }
    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VillaDTO> GetVilla(int id)
    {
        if (id == 0)
        {
            _logger.Log($"GetVilla error with id {id}", "");
            return BadRequest();
        }
        
        var villa = VillaStore.villasList.FirstOrDefault(x => x.Id == id);

        if (villa == null)
        {
            return NotFound();
        }
        
        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }\
        if (VillaStore.villasList.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
        {
            ModelState.AddModelError("CustomError", "Villa already exists");
            return BadRequest(ModelState);
        }
        if (villaDTO == null)
        {
            return BadRequest(villaDTO);
        }

        if (villaDTO.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        villaDTO.Id = VillaStore.villasList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
        VillaStore.villasList.Add(villaDTO);
        
        return CreatedAtRoute("GetVilla", new {id = villaDTO.Id} , villaDTO);
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }
        var villa = VillaStore.villasList.FirstOrDefault(x => x.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        VillaStore.villasList.Remove(villa);
        return NoContent();
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
    {
        if (villaDTO == null || id != villaDTO.Id)
        {
            return BadRequest();
        }

        var villa = VillaStore.villasList.FirstOrDefault(x => x.Id == id);
        villa.Name = villaDTO.Name;
        villa.Sqft = villaDTO.Sqft;
        villa.Occupancy = villaDTO.Occupancy;
        
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "PatchVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PatchVilla(int id, JsonPatchDocument<VillaDTO> villaPatchDocument)
    {
        if (villaPatchDocument == null || id == 0)
        {
            return BadRequest();
        }
        var villa = VillaStore.villasList.FirstOrDefault(x => x.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        villaPatchDocument.ApplyTo(villa, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}