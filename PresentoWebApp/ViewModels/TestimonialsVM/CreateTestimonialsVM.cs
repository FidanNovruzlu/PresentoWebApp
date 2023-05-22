using PresentoWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace PresentoWebApp.ViewModels.TestimonialsVM;
public class CreateTestimonialsVM
{
    public string Name { get; set; } = null!;
    [MaxLength(100)]
    public string? Description { get; set; }
    public string Surname { get; set; } = null!;
    public string? ProfileImageName { get; set; } 
    public IFormFile ProfileImage { get; set; } = null!;
    public int JobId { get; set; }
    public List<Job>? Jobs { get; set; }
}
