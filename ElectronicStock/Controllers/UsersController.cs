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
    public class UsersController : Controller
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

        // GET: Users
        public async Task<IActionResult> Index(string patronymic, string name, string surname, string email, string gender, UserSort sort = UserSort.EmailAsc)
        {
            IQueryable<User> users = _userManager.Users;

            if (!String.IsNullOrEmpty(patronymic))
            {
                users = users.Where(x => x.Patronymic.Contains(patronymic));
            }

            if (!String.IsNullOrEmpty(name))
            {
                users = users.Where(x => x.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(surname))
            {
                users = users.Where(x => x.Surname.Contains(surname));
            }

            if (!String.IsNullOrEmpty(email))
            {
                users = users.Where(x => x.Email.Contains(email));
            }

            if (!String.IsNullOrEmpty(gender))
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

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(UserSort)).Cast<UserSort>()
                .Select(x => new SelectListItem
                {
                    Text = x.GetType()
            .GetMember(x.ToString())
            .FirstOrDefault()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName(),
                    Value = x.ToString(),
                    Selected = (x == sort)
                }).ToList();

            ViewBag.Name = name;
            ViewBag.Surname = surname;
            ViewBag.Patronymic = patronymic;
            ViewBag.Email = email;

            return View(await users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Patronymic,RegistrationDate,Email,PhoneNumber,Gender,Address,BirthdayDate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
