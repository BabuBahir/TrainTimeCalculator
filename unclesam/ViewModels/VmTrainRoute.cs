using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace unclesam.Models
{
    public class VmTrainRoute
    {         
        public string TrainName { get; set; }
        public string Route { get; set; }
        public string Depart { get; set; }
        public string Arrival { get; set; }
         


        public virtual string JourneyHours { get; set; }
        public virtual string WaitHours { get; set; }
        public virtual string Destination { get; set; }
        public virtual string Source { get; set; }
    }
 
}

