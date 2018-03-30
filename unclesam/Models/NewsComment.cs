using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace unclesam.Models
{
    public class NewsComment
    {         
        public int Id { get; set; }
        public string CommentTitle { get; set; }
        public string CommentText { get; set; }

        [ForeignKey("User")]
        public virtual int UserId  { get; set; }

        [NotMapped]
        public int NewsItemId { get; set; }
        [NotMapped]
        public int IsApproved { get; set; }
        [NotMapped]
        public int CustomerId { get; set; }
        [NotMapped]
        public int StoreId { get; set; }
        [NotMapped]
        public DateTime CreatedOnUtc { get; set; }
    }
 
}
