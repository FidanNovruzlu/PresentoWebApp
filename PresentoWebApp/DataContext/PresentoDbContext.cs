using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PresentoWebApp.Models;

namespace PresentoWebApp.DataContext;
public class PresentoDbContext : IdentityDbContext<AppUser>
{
	public PresentoDbContext(DbContextOptions<PresentoDbContext> options):base(options)
	{

	}
	public DbSet<Testimonials> Testimonials { get; set;}
	public DbSet<Job> Jobs { get; set;}
}
