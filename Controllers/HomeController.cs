using System.Diagnostics;
using System.Drawing.Printing;
using JKNews.Data;
using JKNews.Models;
using JKNews.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JKNews.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString = "",
        int pageNumber = 1, int pageSize = 3)
    {
        var posts = from s in _context.Posts
                       select s;

        if (!String.IsNullOrEmpty(searchString))
        {
            posts = posts.Where(s => s.Title.Contains(searchString));
        }

        return View(await PaginatedList<Post>.CreateAsync(posts, pageNumber, pageSize));
    }

    public IEnumerable<Post> GetAllPosts(int page, int pageSize)
    {
        return _context.Posts
            .AsNoTracking()
            .Skip(page * pageSize)
            .Take(pageSize)
            .OrderByDescending(x => x.CreatedAt);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
