using System.ComponentModel.DataAnnotations;

namespace DotNetApplication.Models.Dto;

public class VillaNumberUpdateDTO
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string SpecialDetails { get; set; }
    public VillaDTO Villa { get; set; }
}