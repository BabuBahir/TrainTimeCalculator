using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using unclesam.Models;
 

namespace unclesam.Controllers
{
    public class UserController : Controller
    {
        private UserContext _dbcontext;
        private readonly IMapper _mapper;
      

        public UserController(
            UserContext dbcontext,
            IMapper mapper              
            )
        {
            _dbcontext = dbcontext;
            this._mapper = mapper;             
        }

        [Route("")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_dbcontext.TrainSchedule.ToList());
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
            var alltrainsFromSource = _dbcontext.TrainSchedule.Where(x => x.Station.Equals(fromStation));

            var alltrainsToDestination = _dbcontext.TrainSchedule.Where(x => x.Station.Equals(toStation));


            // first find direct Trains
         //   var directTrain = alltrainsFromSource.Intersect(alltrainsToDestination);

            var directTrainslist =  from Item1 in alltrainsFromSource
                                join Item2 in alltrainsToDestination
                                on Item1.TrainName equals Item2.TrainName // join on some property
                                    select new TrainSchedule{
                                            Id=  Item1.Id,
                                            Station = Item1.Station,
                                            TrainName= Item1.TrainName,
                                             TrainNo=Item2.TrainNo
                                    };

            VmTrain DirectTrains = new VmTrain();

            List<VmTrainRoute> trainroutes = new List<VmTrainRoute>();
            //then check if it is direct train to destination 

            // these are the direct trains
            foreach (var item in directTrainslist)
            {                  
                    VmTrainRoute trainRoute = new VmTrainRoute();
                    trainRoute.TrainName = item.TrainName.Trim() + "-> " + item.Station.Trim() + " to " + toStation.Trim();

                    DateTime Depart = _dbcontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == fromStation).FirstOrDefault().ScheduleTime;
                    DateTime Arrival = _dbcontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == toStation).FirstOrDefault().ScheduleTime;

                    trainRoute.Depart = Depart.ToShortTimeString() +" ," + Depart.DayOfWeek;
                    trainRoute.Arrival = Arrival.ToShortTimeString() + " ," + Arrival.DayOfWeek;    
                                  
                trainRoute.WaitHours = "0 Hours";
                trainroutes.Add(trainRoute);
                //trainroutes.  // train.JourneyHours = trainRoute.Arrival.Subtract(trainRoute.Depart);                 
            }

            // assignment
            DirectTrains.TrainRoutes = trainroutes;


            // logic for indirect trains *************************************


            VmTrain IndirectTrains = new VmTrain();
            List<VmTrainRoute> IndirectTrainRoutes = new List<VmTrainRoute>();
            // step 1. get all destination which are neither source nor destination
            // step 1. calculate incomeing and outgoing trains for there

            var allStations = _dbcontext.TrainSchedule.Select(x => x.Station.Trim()).Distinct();

            foreach (var viastation in allStations) {
                if (viastation != fromStation && viastation != toStation)  //mumbai 
                {
                    foreach (var train in alltrainsFromSource)
                    {
                        var alltrainsToViaStationFromSource =  _dbcontext.TrainSchedule.Where(x => x.TrainNo == train.TrainNo).ToList();

                        foreach (var item in alltrainsToViaStationFromSource)
                        {
                             
                        }
                    }                     
                     
                }
            }
            IndirectTrains.TrainRoutes = IndirectTrainRoutes;





           // return json
            dynamic obj = new ExpandoObject();
            obj.DirectTrains = DirectTrains;
            obj.IndirectTrains = IndirectTrains;             
            return Json(obj);  
         
        }
    }
}

