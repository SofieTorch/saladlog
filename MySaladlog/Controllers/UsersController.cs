using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySaladlog.Models;

namespace MySaladlog
{
    public class UsersController : Controller
    {
        private readonly SaladlogContext _context;

        public UsersController(SaladlogContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);
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
        public async Task<IActionResult> Create([Bind("IdUser,FirstName,LastName,UserName,Password,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("IdUser,FirstName,LastName,UserName,Password,Email")] User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.IdUser))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(short id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }

        // POST: Users/Login
        [HttpPost]
        public IActionResult Login(User users)
        {
            if (users == null)
            {
                return View(users);
            }

            var user = _context.Users
                .FirstOrDefault(m => m.UserName == users.UserName);
            if (user == null)
            {
                return NotFound();
            }
            if (Decrypt(user.Password) != users.Password)
            {
                return View(users);
            }

            return View(nameof(Register));

        }

        // GET: Users/Login
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Register(User user)
        {
            user.Password = Encrypt(user.Password);
            if (ModelState.IsValid)
            {
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }
        public IActionResult RegisterUser()
        {
            return View();
        }

        string Encrypt(string data)
        {
            byte[] encryp = System.Text.Encoding.Unicode.GetBytes(data.ToString());
            return Convert.ToBase64String(encryp);
        }

        string Decrypt(string data)
        {
            byte[] decrypt = Convert.FromBase64String(data);

            return System.Text.Encoding.Unicode.GetString(decrypt); ;
        }
    }
}
