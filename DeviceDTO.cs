using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class DeviceDto
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}