using System.ComponentModel.DataAnnotations;

namespace APBD999999;

public class EmployeeDetailsDto
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
    public decimal Salary { get; set; }

    [Required]
    public PositionDto Position { get; set; }

    [Required]
    public DateTime HireDate { get; set; }
}