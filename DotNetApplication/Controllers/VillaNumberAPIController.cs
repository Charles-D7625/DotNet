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

[Route("api/VillaNumberAPI")]
[ApiController]
public class VillaNumberAPIController : ControllerBase
{
    private readonly IVillaNumberRepository _villaNumberRepository;
    private readonly IVillaRepository _villaRepository;
    private readonly IMapper _mapper;
    
    protected APIResponse _response;
    public VillaNumberAPIController(
        IVillaNumberRepository villaNumberRepository,
        IVillaRepository villaRepository,
        IMapper mapper)
    {
        _villaNumberRepository = villaNumberRepository;
        _villaRepository = villaRepository;
        _mapper = mapper;
        this._response = new APIResponse();
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetVillasNumber()
    {
        try
        {
            IEnumerable<VillaNumber> villasNumber = await _villaNumberRepository.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villasNumber);
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
    
    [HttpGet("{id:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _villaNumberRepository.GetAsync(v => v.VillaNo == id);

            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody]VillaNumberCreateDTO createVillaNumberDTO)
    {
        try
        {
            if (await _villaNumberRepository.GetAsync(v => v.VillaNo == createVillaNumberDTO.VillaNo) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Number already exists");
                return BadRequest(ModelState);
            }

            if (await _villaRepository.GetAsync(v => v.Id == createVillaNumberDTO.VillaNo) == null)
            {
                ModelState.AddModelError("CustomError", "Villa Id is invalid");
                return BadRequest(ModelState);
            }
            
            if (createVillaNumberDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(createVillaNumberDTO);
            }

            VillaNumber villaNumber = _mapper.Map<VillaNumber>(createVillaNumberDTO);

            await _villaNumberRepository.CreateAsync(villaNumber);

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
        }
        catch (Exception e)
        {
            _response.IsSuccessStatusCode = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _villaNumberRepository.RemoveAsync(villaNumber);

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

    [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateVillaNumberDTO)
    {
        try
        {
            if (updateVillaNumberDTO == null || id != updateVillaNumberDTO.VillaNo)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepository.GetAsync(v => v.Id == updateVillaNumberDTO.VillaNo) == null)
            {
                ModelState.AddModelError("CustomError", "Villa Id is invalid");
                return BadRequest(ModelState);
            }
            
            VillaNumber model = _mapper.Map<VillaNumber>(updateVillaNumberDTO);

            await _villaNumberRepository.UpdateNumberAsync(model);

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
}