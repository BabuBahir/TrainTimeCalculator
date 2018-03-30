using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using unclesam.Models;

namespace unclesam.Controllers
{
    public class UserController : Controller
    {
        private UserContext usercontext;

        public UserController(UserContext context)
        {
            usercontext = context;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(usercontext.Users.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                usercontext.Users.Add(user);
                usercontext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet("/Countries")]
        public async Task<IActionResult> Countries()
        {
            var allcountries = usercontext.Country.ToList();
            return View("Views/Home/Countries.cshtml", allcountries);
        }

        [HttpGet("/CreateNewsComment")]
        public async Task<IActionResult> CreateNewsComment()
        {
            try
            {
                var NewsComment = new NewsComment
                {
                    CommentTitle = "sanjib is a good boy",
                    CommentText = "and sanjib is lazy ",
                    NewsItemId = 1,
                    IsApproved = 1,
                    StoreId = 1,
                    CustomerId = 1,
                    CreatedOnUtc = DateTime.Now
                };

                usercontext.NewsComment.Add(NewsComment);
                var result = usercontext.SaveChanges();
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

