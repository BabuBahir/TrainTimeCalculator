using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace unclesam.Models
{
    public class VmNewsComment
    {         
        public int Id { get; set; }
        public string CommentTitle { get; set; }
        public string CommentText { get; set; }
        
        public virtual User User { get; set; }
    }
 
}
