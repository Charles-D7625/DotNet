using System.Net;
using AutoMapper;
using DotNetApplication.Models;
using DotNetApplication.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using DotNetApplication.Data;
using DotNetApplication.Logging;
using DotNetApplication.Repository;
using DotNetApplication.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace DotNetApplication.Controllers;

[Route("api/VillaAPI")]
[ApiController]
public class VillaAPIController : ControllerBase
{
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    
    protected APIResponse _response;
    public VillaAPIController(IVillaRepository villaRepository, IMapper mapper)
    {
        _villaRepository = villaRepository;
        _mapper = mapper;
        this._response = new APIResponse();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            IEnumerable<Villa> villas = await _villaRepository.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDTO>>(villas);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _villaRepository.GetAsync(v => v.Id == id);

            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDTO createVillaDTO)
    {
        try
        {
            if (await _villaRepository.GetAsync(v => v.Name.ToLower() == createVillaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa already exists");
                return BadRequest(ModelState);
            }

            if (createVillaDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(createVillaDTO);
            }

            Villa villa = _mapper.Map<Villa>(createVillaDTO);

            await _villaRepository.CreateAsync(villa);

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _villaRepository.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _villaRepository.RemoveAsync(villa);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccessStatusCode = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateVillaDTO)
    {
        try
        {
            if (updateVillaDTO == null || id != updateVillaDTO.Id)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Villa model = _mapper.Map<Villa>(updateVillaDTO);

            await _villaRepository.UpdateAsync(model);

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccessStatusCode = true;
            return Ok(_response);
        }
        catch (Exception e)
        {
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpPatch("{id:int}", Name = "PatchVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> villaPatchDocument)
    {
        if (villaPatchDocument == null || id == 0)
        {
            return BadRequest();
        }
        var villa = _villaRepository.GetAsync(x => x.Id == id, tracking:false);
        
        if (villa == null)
        {
            return NotFound();
        }
        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
        
        villaPatchDocument.ApplyTo(villaDTO, ModelState);
        Villa model = _mapper.Map<Villa>(villaPatchDocument);
        
        _villaRepository.UpdateAsync(model);
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }
}