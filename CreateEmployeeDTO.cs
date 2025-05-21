using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class CreateEmployeeDto
{
    [Required]
    public string PassportNumber { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    public int PositionId { get; set; }

    [Required]
    public DateTime HireDate { get; set; }
}