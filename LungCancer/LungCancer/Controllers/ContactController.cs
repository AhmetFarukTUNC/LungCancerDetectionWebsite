using LungCancer.Data;
using LungCancer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LungCancer.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Contact
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                _context.ContactMessages.Add(model);
                await _context.SaveChangesAsync();

                ViewBag.Success = "Mesajınız başarıyla gönderildi!";
                return View();
            }

            return View(model);
        }
    }
}
