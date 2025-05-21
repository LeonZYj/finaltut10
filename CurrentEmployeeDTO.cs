using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class CurrentEmployeeDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }
}