using SimpleCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace SimpleCMS.DAL
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext()
            : base("ApplicationContext")
        {
        }

        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<ApiAccounts> ApiAccounts { get; set; }

        public static ApplicationContext Create()
        {
            return new ApplicationContext(); 
        }
    }
}