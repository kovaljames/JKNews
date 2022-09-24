using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JKNews.Data;
using JKNews.Models;
using Microsoft.AspNetCore.Authorization;
using JKNews.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using JKNews.Extensions;

namespace JKNews.Controllers;

public class PostsController : Controller
{
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Posts
    public async Task<IActionResult> Index()
    {
            return _context.Posts != null ? 
                        View(await _context.Posts.ToListAsync()) :
                        Problem("Entity set 'DataContext.Posts'  is null.");
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // GET: Posts/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        ViewBag.Tags = await _context.Tags.ToListAsync();
        ViewBag.User = User.Identity?.Name;
        return View();
    }

    // POST: Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(PostViewModel model)
    {
        ClaimsPrincipal currentUser = User;
        var currentUserID = Convert.ToInt32(
            currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var post = new Post
        {
            Title = model.Title,
            Desc = model.Desc,
            Slug = StringExtensions.Slugify(model.Title),
            CategoryId = model.CategoryId,
            UserId = currentUserID,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Tags = new List<Tag>()
        };

        if (model.TagsIds != null)
        {
            foreach (int tagId in model.TagsIds)
            {
                var tag = _context.Tags.Single(t => t.Id == tagId);
                post.Tags.Add(tag);
            }
        }

        if (ModelState.IsValid)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var post = await _context.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        ViewBag.DefaultCategory = categories.FirstOrDefault(x => x.Id == post.CategoryId)?.Name;

        ViewBag.Tags = await _context.Tags.ToListAsync();
        post.TagsIds = post.Tags?.Select(x => x.Id).ToList();

        ViewBag.User = User.Identity?.Name;
        return View(post);
    }

    // POST: Posts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id, Post post)
    {
        ClaimsPrincipal currentUser = User;
        var currentUserID = Convert.ToInt32(
            currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        post.UpdatedAt = DateTime.Now;

        if (post.UserId != currentUserID)
            return Unauthorized();

        if (id != post.Id)
            return NotFound();

        /*if (post.TagsIds != null && post.TagsIds.Count > 0) {
            List<int> TagsIdsFixed = post.Tags.Select(x => x.Id).ToList();
            if (post.TagsIds != null && TagsIdsFixed != null && post.TagsIds != TagsIdsFixed)
            {
                foreach (int tagId in post.TagsIds)
                {
                    if (!TagsIdsFixed.Contains(tagId))
                    {
                        var tag = _context.Tags.Single(t => t.Id == tagId);
                        post.Tags?.Add(tag);
                    }
                }
                foreach (int tagDBId in TagsIdsFixed)
                {
                    if (!post.TagsIds.Contains(tagDBId))
                    {
                        var tag = _context.Tags.Single(t => t.Id == tagDBId);
                        post.Tags?.Remove(tag);
                    }
                }
            }
        }*/

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Posts == null)
        {
            return Problem("Entity set 'DataContext.Posts'  is null.");
        }
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
        return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
