using System;

namespace unclesam.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public RoleType RoleTyp { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }

    public enum RoleType
    {
        Admin,
        User
    }
}
