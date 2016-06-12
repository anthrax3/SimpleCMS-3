using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SimpleCMS.Models
{
    public class ApiAccounts
    {
        [Key]
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string RequestURL { get; set; }
    }
}