using System;
using System.Collections.Generic;

namespace unclesam.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public List<Role> Roles { get; set; }
    }
}
