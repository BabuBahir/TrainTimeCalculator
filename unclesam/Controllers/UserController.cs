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

        public async Task<IActionResult> GetTrainsBetweenStation(string fromStation , string toStation)
        {
            //first get all trains
            var alltrainsFromSource = usercontext.TrainSchedule.Where(x => x.Station.Equals(fromStation));

            var alltrainsToDestination = usercontext.TrainSchedule.Where(x => x.Station.Equals(toStation));


            // first find direct Trains
         //   var directTrain = alltrainsFromSource.Intersect(alltrainsToDestination);

            var directTrains =  from Item1 in alltrainsFromSource
                            join Item2 in alltrainsToDestination
                            on Item1.TrainName equals Item2.TrainName // join on some property
                                select new TrainSchedule{
                                        Id=  Item1.Id,
                                        Station = Item1.Station,
                                        TrainName= Item1.TrainName,
                                         TrainNo=Item2.TrainNo
                                };

            VmTrain Trains = new VmTrain();

            List<VmTrainRoute> trainroutes = new List<VmTrainRoute>();
            //then check if it is direct train to destination 
            foreach (var item in directTrains)
            {                  
                    VmTrainRoute trainRoute = new VmTrainRoute();
                    trainRoute.TrainName = item.TrainName.Trim() + "->" + item.Station.Trim() + " to " + toStation.Trim();
                    trainRoute.Depart = usercontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == fromStation).FirstOrDefault().ScheduleTime.ToShortTimeString();
                    trainRoute.Arrival = usercontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == toStation).FirstOrDefault().ScheduleTime.ToShortTimeString();
                 
                trainroutes.Add(trainRoute);
                //trainroutes.  // train.JourneyHours = trainRoute.Arrival.Subtract(trainRoute.Depart);                 
            }

            // assignment
            Trains.TrainRoutes = trainroutes;

            return Json(Trains);
        }
    }
}

