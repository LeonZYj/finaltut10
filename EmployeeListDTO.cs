using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class EmployeeListDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }
}
