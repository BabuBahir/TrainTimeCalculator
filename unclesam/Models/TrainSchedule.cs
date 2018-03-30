using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace unclesam.Models
{
    public class TrainSchedule
    {         
        public int Id { get; set; }
       
        public int TrainNo { get; set; }

        public string TrainName { get; set; }

        public string Station { get; set; }

        public DateTime ScheduleTime { get; set; }
    }
 
}
