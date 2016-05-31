using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleCMS.Models
{
    public class Comments
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string content { get; set; }

        [Required]
        public bool hasChild { get; set; }

        public int childCommentID { get; set; }

        public virtual ICollection<Posts> posts { get; set; }
    }
}