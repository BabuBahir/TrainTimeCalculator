using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace unclesam.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "User Location is required")]
        public string Location { get; set; }

        public List<Role> Roles { get; set; }
    }
}
