using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers;

public class VillaNumberController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;

    public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _mapper = mapper;
    }

    public async Task<IActionResult> IndexVillaNumber()
    {
        List<VillaNumberDTO> list = new List<VillaNumberDTO>();
        var response = await _villaNumberService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccessStatusCode)
        {
            list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
        }
        else
        {
            if (response.ErrorMessages.Count > 0)
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
            }
        }
        return View(list);
    }
    
    public async Task<IActionResult> CreateVillaNumber()
    {
        VillaNumberCreateVM villaNumberVM = new VillaNumberCreateVM();
        
        var response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccessStatusCode)
        {
            villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
        }
        return View(villaNumberVM);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            if (response.ErrorMessages.Count > 0)
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
            }
        }
        
        var resp = await _villaService.GetAllAsync<APIResponse>();
        if (resp != null && resp.IsSuccessStatusCode)
        {
            model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(resp.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        
        return View(model);
    }
    
    public async Task<IActionResult> UpdateVillaNumber(int villaNo)
    {
        VillaNumberUpdateVM villaNumberUpdateVM = new VillaNumberUpdateVM();
        var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
        if (response != null && response.IsSuccessStatusCode)
        {
            VillaNumberDTO villaNumberDTO = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(villaNumberDTO);
        }
        
        response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccessStatusCode)
        {
            villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(villaNumberUpdateVM);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaNumberDto)
    {
        if (ModelState.IsValid)
        {
            var response = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberDto.VillaNumber);
            if (response != null && response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            if (response.ErrorMessages.Count > 0)
            {
                ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
            }
        }
        
        var resp = await _villaService.GetAllAsync<APIResponse>();
        if (resp != null && resp.IsSuccessStatusCode)
        {
            villaNumberDto.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(resp.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        
        return View(villaNumberDto);
    }
    
    public async Task<IActionResult> DeleteVillaNumber(int villaNo)
    {
        VillaNumberDeleteVM villaNumberDeleteVM = new VillaNumberDeleteVM();
        var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
        if (response != null && response.IsSuccessStatusCode)
        {
            VillaNumberDTO villaNumberDTO = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            villaNumberDeleteVM.VillaNumber = villaNumberDTO;
        }
        
        response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccessStatusCode)
        {
            villaNumberDeleteVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                (Convert.ToString(response.Result)).Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(villaNumberDeleteVM);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM villaNumberDto)
    {
        var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNumberDto.VillaNumber.VillaNo);
        if (response != null && response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(IndexVillaNumber));
        }
        return View(villaNumberDto);
    }
}