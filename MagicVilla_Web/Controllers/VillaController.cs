using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;
    
    public VillaController(IVillaService villaService, IMapper mapper)
    {
        _villaService = villaService;
        _mapper = mapper;
    }

    public async Task<IActionResult> IndexVilla()
    {
        List<VillaDTO> list = new List<VillaDTO>();
        var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccessStatusCode)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVilla()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccessStatusCode)
            {
                TempData["success"] = "Villa has been created successfully!";
                return RedirectToAction("IndexVilla");
            }
        }
        TempData["error"] = "Error encountered while creating villa!";
        return View(model);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVilla(int villaId)
    {
        var response = await _villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccessStatusCode)
        {
            
            VillaDTO villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(_mapper.Map<VillaUpdateDTO>(villaDTO));
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaDTO)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaService.UpdateAsync<APIResponse>(villaDTO, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccessStatusCode)
            {
                TempData["success"] = "Villa has been updated successfully!";
                return RedirectToAction("IndexVilla");
            }
        }
        TempData["error"] = "Error encountered while updating villa!";
        return View(villaDTO);
    }
    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(int villaId)
    {
        var response = await _villaService.GetAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccessStatusCode)
        {
            VillaDTO villaDTO = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
            return View(villaDTO);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteVilla(VillaDTO villaDTO)
    {
        var response = await _villaService.DeleteAsync<APIResponse>(villaDTO.Id, HttpContext.Session.GetString(SD.SessionToken));
        if (response != null && response.IsSuccessStatusCode)
        {
            TempData["success"] = "Villa has been deleted successfully!";
            return RedirectToAction(nameof(IndexVilla));
        }
        TempData["error"] = "Error encountered while deleting villa!";
        return View(villaDTO);
    }
}