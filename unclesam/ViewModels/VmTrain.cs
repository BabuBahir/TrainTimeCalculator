using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace unclesam.Models
{
    public class VmTrain
     {         
       public virtual List<VmTrainRoute> TrainRoutes { get; set; }
        public virtual DateTime JourneyHours { get; set; }
        public virtual DateTime WaitHours { get; set; }
    }
 
}
