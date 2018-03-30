using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using unclesam.Models;
 

namespace unclesam.Controllers
{
    public class UserController : Controller
    {
        private UserContext usercontext;
        private readonly IMapper _mapper;
      

        public UserController(
            UserContext context,
            IMapper mapper              
            )
        {
            usercontext = context;
            this._mapper = mapper;             
        }

        [Route("")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(usercontext.TrainSchedule.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        usercontext.Users.Add(user);
        //        usercontext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}
           
    }
}

