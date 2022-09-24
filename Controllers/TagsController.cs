﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JKNews.Data;
using JKNews.Models;
using Microsoft.AspNetCore.Authorization;

namespace JKNews.Controllers;

public class TagsController : Controller
{
    private readonly AppDbContext _context;

    public TagsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Tags
    public async Task<IActionResult> Index()
    {
        return _context.Tags != null ? 
                    View(await _context.Tags.ToListAsync()) :
                    Problem("Entity set 'DataContext.Tags'  is null.");
    }

    // GET: Tags/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Tags == null)
        {
            return NotFound();
        }

        var tag = await _context.Tags
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tag == null)
        {
            return NotFound();
        }

        return View(tag);
    }

    // GET: Tags/Create
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tags/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create([Bind("Id,Name,Slug,Desc")] Tag tag)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    // GET: Tags/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Tags == null)
        {
            return NotFound();
        }

        var tag = await _context.Tags.FindAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        return View(tag);
    }

    // POST: Tags/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Slug,Desc")] Tag tag)
    {
        if (id != tag.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(tag.Id))
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
        return View(tag);
    }

    // GET: Tags/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Tags == null)
        {
            return NotFound();
        }

        var tag = await _context.Tags
            .FirstOrDefaultAsync(m => m.Id == id);
        if (tag == null)
        {
            return NotFound();
        }

        return View(tag);
    }

    // POST: Tags/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Tags == null)
        {
            return Problem("Entity set 'DataContext.Tags'  is null.");
        }
        var tag = await _context.Tags.FindAsync(id);
        if (tag != null)
        {
            _context.Tags.Remove(tag);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TagExists(int id)
    {
        return (_context.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
