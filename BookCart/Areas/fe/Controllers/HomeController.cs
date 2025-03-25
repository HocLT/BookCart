using BookCart.Data;
using BookCart.Models;
using Humanizer;
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

        // fe/home/bookdeails/id
        public async Task<IActionResult> BookDetails(int id)
        {
            Book? book = await _ctx.Books.SingleOrDefaultAsync(b => b.Id == id);
            return View(book);
        }

        // fe/home/books
        // princeRange: 10 - 30
        public async Task<IActionResult> Books(int? c, string? priceRange, string? author)
        {
            List<Book> books = await _ctx.Books.ToListAsync();
            var categories = await _ctx.Categories.ToListAsync();
            ViewBag.Categories = categories;
            

            List<decimal> prices = books
                .Select(b => b.Price!.Value)
                .OrderBy(b => b)
                .ToList();
            ViewBag.PriceMin = prices[0];
            ViewBag.PriceMax = prices[prices.Count-1];

            List<string> authors = books
                .Select(b => b.Author!)
                .Distinct()
                .ToList();

            ViewBag.Authors = authors;


            if (c != null)
            {
                books = await _ctx.Books
                    .Where(b => b.CategoryId == c)
                    .ToListAsync();
            }

            if (priceRange != null && author != null)
            {
                string[] priceList = priceRange.Split("-");
                decimal min = Convert.ToDecimal(priceList[0].Trim());
                decimal max = Convert.ToDecimal(priceList[1].Trim());
                books = books.Where(b => b.Price!.Value >= min && b.Price!.Value <= max && b.Author == author)
                    .ToList();
            }
            else if (priceRange != null)
            {
                string[] priceList = priceRange.Split("-");
                decimal min = Convert.ToDecimal(priceList[0].Trim());
                decimal max = Convert.ToDecimal(priceList[1].Trim());
                books = books.Where(b => b.Price!.Value >= min && b.Price!.Value <= max)
                    .ToList();
            }
            else if (author != null)
            {
                books = books.Where(b => b.Author == author)
                    .ToList();
            }


            return View(books);
        }
    }
}
