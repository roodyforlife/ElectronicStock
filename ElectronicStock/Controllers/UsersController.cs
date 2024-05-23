using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicStock.BaseContext;
using ElectronicStock.Models;
using Microsoft.AspNetCore.Identity;
using ElectronicStock.Enums;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ElectronicStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string patronymic, string name, string surname, string email, string gender, UserSort sort = UserSort.EmailAsc)
        {
            IQueryable<User> users = _userManager.Users;

            if (!string.IsNullOrEmpty(patronymic))
            {
                users = users.Where(x => x.Patronymic.Contains(patronymic));
            }

            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(surname))
            {
                users = users.Where(x => x.Surname.Contains(surname));
            }

            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(x => x.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                users = users.Where(x => x.Gender == gender);
            }

            switch (sort)
            {
                case UserSort.EmailDesc:
                    users = users.OrderByDescending(x => x.Email);
                    break;
                case UserSort.BirthdayAsc:
                    users = users.OrderBy(x => x.BirthdayDate);
                    break;
                case UserSort.BirthdayDesc:
                    users = users.OrderByDescending(x => x.BirthdayDate);
                    break;
                case UserSort.NameAsc:
                    users = users.OrderBy(x => x.Name);
                    break;
                case UserSort.SurnameAsc:
                    users = users.OrderBy(x => x.Surname);
                    break;
                case UserSort.PatronymicAsc:
                    users = users.OrderBy(x => x.Patronymic);
                    break;
                case UserSort.GenderAsc:
                    users = users.OrderBy(x => x.Gender);
                    break;
                case UserSort.GenderDesc:
                    users = users.OrderByDescending(x => x.Gender);
                    break;
                default:
                    users = users.OrderBy(x => x.Email);
                    break;
            }

            return await users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([Bind("Id,Name,Surname,Patronymic,RegistrationDate,Email,PhoneNumber,Gender,Address,BirthdayDate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var shopCards = _context.ShopCards.Where(x => x.UserId == user.Id);
            _context.ShopCards.RemoveRange(shopCards);
            _context.AppUsers.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TestUserExists(string id)
        {
            return _context.AppUsers.Any(e => e.Id == id);
        }
    }
}
