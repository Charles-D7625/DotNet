using AutoMapper;
using MagicVilla_MVC.Models;
using MagicVilla_MVC.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_MVC.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_MVC.Controllers;

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
        var response = await _villaService.GetAllAsync<APIResponse>();
        if (response != null && response.IsSuccessStatusCode)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
        }
        return View(list);
    }
}