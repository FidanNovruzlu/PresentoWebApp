using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentoWebApp.DataContext;
using PresentoWebApp.Models;
using PresentoWebApp.ViewModels.TestimonialsVM;

namespace PresentoWebApp.Areas.Admin.Controllers;
[Area("Admin")]
public class TestimonialsController : Controller
{
    private readonly PresentoDbContext _context;
    private readonly IWebHostEnvironment _environment;
    public TestimonialsController(PresentoDbContext context,IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _environment = webHostEnvironment;
    }

    public async  Task<IActionResult> Index()
    {
        List<Testimonials> testimonials= await _context.Testimonials.Include(t=>t.Job).ToListAsync();
        return View(testimonials);
    }
    public async Task<IActionResult> Create()
    {
        CreateTestimonialsVM testimonialsVM = new CreateTestimonialsVM()
        {
            Jobs=await _context.Jobs.ToListAsync(),
        };
        return View(testimonialsVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTestimonialsVM testimonialsVM)
    {
        if(!ModelState.IsValid)
        {
            testimonialsVM.Jobs =await _context.Jobs.ToListAsync();
            return View(testimonialsVM);
        }
        Testimonials testimonials=new Testimonials()
        {
            Name=testimonialsVM.Name,
            Surname=testimonialsVM.Surname,
            Description=testimonialsVM.Description,
            JobId=testimonialsVM.JobId
        };
        if(!testimonialsVM.ProfileImage.ContentType.Contains("image/") && testimonialsVM.ProfileImage.Length /1024> 2048)
        {
            ModelState.AddModelError("", "Incorrect image size or type!!!");
            testimonialsVM.Jobs = await _context.Jobs.ToListAsync();
            return View(testimonialsVM);
        }
        string newFileName=Guid.NewGuid().ToString() +testimonialsVM.ProfileImage.FileName;
        string path = Path.Combine(_environment.WebRootPath, "assets", "img", "testimonials", newFileName);

        using(FileStream fileStream=new FileStream(path, FileMode.CreateNew))
        {
            await testimonialsVM.ProfileImage.CopyToAsync(fileStream);
        }
        testimonials.ProfileImageName = newFileName;
        await _context.Testimonials.AddAsync(testimonials);
        await  _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Read(int id)
    {
        Testimonials? testimonials=await _context.Testimonials.Include(t=>t.Job).FirstOrDefaultAsync(t=>t.Id==id);
        if(testimonials == null) return NotFound();
         
        return View(testimonials);
    }

    public async Task<IActionResult> Update(int id)
    {
        Testimonials? testimonials= await _context.Testimonials.FindAsync(id);
        if(testimonials == null) return NotFound();

        UpdateTestimonialsVM updateTestimonialsVM= new UpdateTestimonialsVM()
        {
            Jobs= await _context.Jobs.ToListAsync(),
            Name= testimonials.Name,
            Surname= testimonials.Surname,
            Description= testimonials.Description,
            JobId= testimonials.JobId,
            ProfileImageName= testimonials.ProfileImageName,
        };
        return View(updateTestimonialsVM);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int id, UpdateTestimonialsVM testimonialsVM)
    {
        if (!ModelState.IsValid)
        {
            testimonialsVM.Jobs = await _context.Jobs.ToListAsync();
            return View(testimonialsVM);
        }
        Testimonials? testimonials = await _context.Testimonials.FindAsync(id);
        if (testimonials == null) return NotFound();

        if(testimonialsVM.ProfileImage != null)
        {
            string newFileName = Guid.NewGuid().ToString() + testimonialsVM.ProfileImage.FileName;
            string path = Path.Combine(_environment.WebRootPath, "assets", "img", "testimonials", testimonials.ProfileImageName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await testimonialsVM.ProfileImage.CopyToAsync(fileStream);
            }
        }
        testimonials.Surname=testimonialsVM.Surname;
        testimonials.Name=testimonialsVM.Name;
        testimonials.Description = testimonialsVM.Description;
        testimonials.JobId=testimonialsVM.JobId;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Testimonials? testimonials=await _context.Testimonials.FindAsync(id);
        if(testimonials == null) return NotFound();

        string path = Path.Combine(_environment.WebRootPath, "assets", "img", "testimonials", testimonials.ProfileImageName);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        _context.Testimonials.Remove(testimonials);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
