using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
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
            try
            {
                //first get all trains            

                var directTrainslist = GetDirectTrains(fromStation, toStation);

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

                    trainRoute.Depart = Depart.ToShortTimeString() + " " + Depart.ToString("tt", CultureInfo.InvariantCulture) + " ," + Depart.DayOfWeek;
                    trainRoute.Arrival = Arrival.ToShortTimeString() + " " + Arrival.ToString("tt", CultureInfo.InvariantCulture) + " ," + Arrival.DayOfWeek;

                    trainRoute.JourneyHours = ((Arrival - Depart).Hours).ToString();
                    trainroutes.Add(trainRoute);
                    //trainroutes.  // train.JourneyHours = trainRoute.Arrival.Subtract(trainRoute.Depart);                 
                }

                // assignment
                DirectTrains.TrainRoutes = trainroutes;


                // logic for indirect trains *************************************


                VmTrain IndirectTrains = new VmTrain();
                List<VmTrainRoute> IndirectTrainRoutesPhase1 = new List<VmTrainRoute>();
                List<VmTrainRoute> IndirectTrainRoutesPhase2 = new List<VmTrainRoute>();

                // step 1. get all destination which are neither source nor destination
                // step 1. calculate incomeing and outgoing trains for there

                var allStations = _dbcontext.TrainSchedule.Select(x => x.Station.Trim()).Distinct();

                List<string> viastations = new List<string>();

                foreach (var viastation in allStations)
                {
                    if (viastation != fromStation && viastation != toStation)  //mumbai 
                    {
                        viastations.Add(viastation);
                        var IndirectrPhase1journey = GetDirectTrains(fromStation, viastation);
                        var IndirectrPhase2journey = GetDirectTrains(viastation, toStation);


                        // delhi -> mumbai
                        foreach (var item in IndirectrPhase1journey)
                        {
                            VmTrainRoute trainRoute = new VmTrainRoute();
                            trainRoute.TrainName = item.TrainName.Trim() + "-> " + item.Station.Trim() + " to " + viastation.Trim();

                            DateTime Depart = _dbcontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == fromStation).FirstOrDefault().ScheduleTime;
                            DateTime Arrival = _dbcontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == viastation).FirstOrDefault().ScheduleTime;

                            trainRoute.Depart = Depart.ToShortTimeString() + " " + Depart.ToString("tt", CultureInfo.InvariantCulture) + " ," + Depart.DayOfWeek;
                            trainRoute.Arrival = Arrival.ToShortTimeString() + " " + Arrival.ToString("tt", CultureInfo.InvariantCulture) + " ," + Arrival.DayOfWeek;

                            trainRoute.dtDepart = Depart;
                            trainRoute.dtArrival = Arrival;

                            trainRoute.Destination = viastation;
                            trainRoute.JourneyHours = ((Arrival - Depart).Hours).ToString();
                            IndirectTrainRoutesPhase1.Add(trainRoute);
                             
                        }
                                               
                        //mumbai -> pune
                        foreach(var item in IndirectrPhase2journey)
                        {                             

                            VmTrainRoute trainRoute = new VmTrainRoute();
                             

                            trainRoute.TrainName = item.TrainName.Trim() + "-> " + item.Station.Trim() + " to " + toStation.Trim();

                            DateTime Depart = _dbcontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == viastation).FirstOrDefault().ScheduleTime;
                            DateTime Arrival = _dbcontext.TrainSchedule.Where(x => x.TrainNo == item.TrainNo && x.Station == toStation).FirstOrDefault().ScheduleTime;

                            trainRoute.Depart = Depart.ToShortTimeString() + " " + Depart.ToString("tt", CultureInfo.InvariantCulture) + " ," + Depart.DayOfWeek;
                            trainRoute.Arrival = Arrival.ToShortTimeString() + " " + Arrival.ToString("tt", CultureInfo.InvariantCulture) + " ," + Arrival.DayOfWeek;

                            trainRoute.dtDepart = Depart;
                            trainRoute.dtArrival = Arrival;

                            trainRoute.Source = item.Station.Trim();                             
                            trainRoute.JourneyHours = ((Arrival - Depart).Hours).ToString();                             
                            IndirectTrainRoutesPhase2.Add(trainRoute);

                        }

                    }
                }


                List<VmTrainRoute> dummyIndirectRoutes = new List<VmTrainRoute>();
                foreach (var src in IndirectTrainRoutesPhase1)
                {
                    foreach (var dest in IndirectTrainRoutesPhase2)
                    {
                        if (src.Destination.Contains(dest.Source))
                        {
                            VmTrainRoute trainRoute = new VmTrainRoute();
                            trainRoute.TrainName = src.TrainName + "  // " +dest.TrainName;
                            trainRoute.Depart = src.Depart +" - " + src.Arrival;
                            trainRoute.Arrival = dest.Depart + " - " + dest.Arrival;

                            trainRoute.JourneyHours = ((Convert.ToInt32(src.JourneyHours) + Convert.ToInt32(dest.JourneyHours) + 1).ToString());  // (int.TryParse(src.JourneyHours) + int.TryParse(src.JourneyHours)).tos; ///  // (((src.dtDepart - src.dtArrival).TotalHours) + ((dest.dtDepart - dest.dtArrival).TotalHours) ).ToString();
                            trainRoute.WaitHours = src.Arrival + " - " + dest.Depart;
                            dummyIndirectRoutes.Add(trainRoute);
                        }
                    }
                }
                 

                IEnumerable<VmTrainRoute> union = IndirectTrainRoutesPhase1.Union(IndirectTrainRoutesPhase2);
                IndirectTrains.TrainRoutes = dummyIndirectRoutes; //union.ToList();





                // return json
                dynamic obj = new ExpandoObject();
                obj.DirectTrains = DirectTrains;
                obj.IndirectTrains = IndirectTrains;      // IndirectTrains   
                return Json(obj);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQueryable<TrainSchedule> GetDirectTrains(string fromStation, string toStation)
        {
            //first get all trains
            var alltrainsFromSource = _dbcontext.TrainSchedule.Where(x => x.Station.Equals(fromStation));

            var alltrainsToDestination = _dbcontext.TrainSchedule.Where(x => x.Station.Equals(toStation));

            var GetDirectTrainList = from Item1 in alltrainsFromSource
                      join Item2 in alltrainsToDestination
                      on Item1.TrainName equals Item2.TrainName // join on some property
                      select new TrainSchedule
                      {
                          Id = Item1.Id,
                          Station = Item1.Station,
                          TrainName = Item1.TrainName,
                          TrainNo = Item2.TrainNo
                      };

            return GetDirectTrainList;


        }
    }
}

