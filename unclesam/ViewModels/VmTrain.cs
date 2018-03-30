using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace unclesam.Models
{
    public class VmTrain
     {         
       public virtual List<VmTrainRoute> TrainRoutes { get; set; }
       
    }
 
}
