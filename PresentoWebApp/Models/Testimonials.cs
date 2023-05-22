using System.ComponentModel.DataAnnotations;

namespace PresentoWebApp.Models;
public class Testimonials
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [Required,MaxLength(150)]
    public string Description { get; set; } = null!;
    public string ProfileImageName { get; set; } = null!;
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
}
