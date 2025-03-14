﻿using BookCart.Data;
using BookCart.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCart.Controllers
{
    public class UserController : Controller
    {
        readonly BookCartDbContext _ctx;

        public UserController(BookCartDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _ctx
                .Users
                .Include(u => u.Role)
                .ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _ctx.Roles.ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _ctx.Users.Add(user);
                await _ctx.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Route("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var roles = await _ctx.Roles.ToListAsync();
            ViewBag.Roles = roles;
            return View();
        }
    }
}
