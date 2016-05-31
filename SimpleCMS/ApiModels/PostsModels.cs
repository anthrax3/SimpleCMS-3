using SimpleCMS.AppClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.ModelBinding;

namespace SimpleCMS.Models
{  
    public class Posts
    {
        public int ID { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public string Category { get; set; }

        [Required]
        public bool Attachment { get; set; }

        public string AttachmentPath { get; set; }

        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}