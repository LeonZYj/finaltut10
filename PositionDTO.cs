using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class PositionDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}