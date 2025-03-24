using BookCart.Data;
using BookCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCart.Areas.fe.Controllers
{
    [Area("fe")]
    public class HomeController : Controller
    {
        readonly BookCartDbContext _ctx;

        public HomeController(BookCartDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            List<Book> featured = await _ctx.Books.Where(b=>b.Features != null && b.Features.Value).Take(2).ToListAsync();
            ViewBag.Featured = featured;
            List<Book> bestSelling = await _ctx.Books.Take(8).ToListAsync();
            List<Book> latest = await _ctx.Books.Take(8).ToListAsync();
            ViewBag.BestSelling = bestSelling;
            ViewBag.Latest = latest;
            return View();
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            Book? book = await _ctx.Books.SingleOrDefaultAsync(b => b.Id == id);
            return View(book);
        }
    }
}
