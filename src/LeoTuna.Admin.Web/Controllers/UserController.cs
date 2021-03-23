using LeoTuna.Admin.Web.Context;
using LeoTuna.Admin.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LeoTuna.Admin.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("/users")]
    public class UserController : Controller
    {
        private AppDbContext AppDbContext { get; }

        public UserController(
            AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await AppDbContext.Users.ToListAsync();
            return View(users);
        }

        [HttpGet("{id:guid}/edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await AppDbContext.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound("User not found.");
            }
            return View(user);
        }

        [HttpPost("{id:guid}/edit")]
        public async Task<IActionResult> Edit(Guid id, User model)
        {
            var user = await AppDbContext.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound("User not found.");
            }

            throw new NotImplementedException();
        }
    }
}
