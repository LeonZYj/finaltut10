using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class UpdateDeviceDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public bool IsEnabled { get; set; }

    [Required]
    public string AdditionalProperties { get; set; }

    [Required]
    public string DeviceTypeName { get; set; }
}