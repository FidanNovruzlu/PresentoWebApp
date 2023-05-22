using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentoWebApp.DataContext;
using PresentoWebApp.Models;
using PresentoWebApp.ViewModels;
using PresentoWebApp.ViewModels.TestimonialsVM;
using System.Diagnostics;

namespace PresentoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly PresentoDbContext _context;
        public HomeController(PresentoDbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            List<Testimonials> testimonials=await _context.Testimonials.Include(t=>t.Job).ToListAsync();
            HomeVM homeVM= new HomeVM()
            {
                Testimonials= testimonials
            };
            return View(homeVM);
        }
    }
}