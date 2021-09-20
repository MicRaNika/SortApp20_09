using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SortApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;



namespace SortApp.Controllers
{
    public class HomeController : Controller
    {
        UsersContext db;
        public HomeController(UsersContext context)
        {
            this.db = context;
            // добавим начальные данные для тестирования
            if (db.Companies.Count() == 0)
            {
                Company netflix = new Company { Name = "Netflix" };
                Company google = new Company { Name = "Google" };
                Company microsoft = new Company { Name = "Microsoft" };
                Company apple = new Company { Name = "Apple" };

                User user1 = new User { Name = "Ivan XXX", Company = netflix, Age = 26 };
                User user2 = new User { Name = "Bogdan Molodets", Company = netflix, Age = 24 };
                User user3 = new User { Name = "Vlad Kovalev", Company = microsoft, Age = 25 };
                User user4 = new User { Name = "Иван Иванов", Company = microsoft, Age = 26 };
                User user5 = new User { Name = "Петр Андреев", Company = microsoft, Age = 23 };
                User user6 = new User { Name = "Micro Nika", Company = google, Age = 22 };
                User user7 = new User { Name = "Олег Кузнецов", Company = google, Age = 25 };
                User user8 = new User { Name = "Андрей Петров", Company = apple, Age = 24 };

                db.Companies.AddRange(netflix, microsoft, google, apple);
                db.Users.AddRange(user1, user2, user3, user4, user5, user6, user7, user8);
                db.SaveChanges();
            }
        }
        public async Task<IActionResult> Index()
        {
            IQueryable<User> users = db.Users.Include(p => p.Company);
            return View(await users.ToListAsync());
        }

        public async Task<IActionResult> AboutCompany()
        {

            return View(await db.Companies.ToListAsync());
        }
        //------------------------------------------------
        public IActionResult AddCompany()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany(Company company)
        {
            db.Companies.Add(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //--------------------------------------------------------

        public IActionResult DetailsCompany(int? id)
        {
            if (id != null)
            {
                Company company = db.Companies.Include(p => p.Users).FirstOrDefault(p => p.Id == id);
                if (company != null)
                    return View(company);
            }
            return NotFound();
        }

        //------------------------------------------
        [HttpGet]
        public async Task<IActionResult> EditCompany(int? id)
        {
            Company company = await db.Companies.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == id);
            if (company == null)
            {
                return RedirectToAction("AboutCompany");
            }
            ViewBag.Users = await db.Users.ToListAsync();
            return View(company);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Company company)
        {
            var users = db.Users.ToList();
           
            foreach (User u in users)
            {
                if (u.CompanyId.Equals(company.Id))
                {
                    db.Users.Update(u);
                }
            }

            db.Companies.Update(company);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //----------------------------------------------------------------------------------------------

        [HttpGet]
        [ActionName("DeleteCompany")]
        public async Task<IActionResult> ConfirmDeleteCompany(int? id)
        {
            if (id != null)
            {
                Company company = await db.Companies.FirstOrDefaultAsync(p => p.Id == id);
                if (company != null)
                    return View(company);
            }
            return NotFound();
        }


        //---------------
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.Include(p => p.Company).FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.Include(p => p.Company).FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
    }

