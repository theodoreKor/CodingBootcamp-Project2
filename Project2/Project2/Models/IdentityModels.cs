using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project2.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project2.Models
{
    
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // my code

        public ApplicationUser()
        {
            MessagesSent = new List<Message>();
            MessagesReceived = new List<Message>();
        }

        public string Avatar { get; set; }
        public UserRoles Role { get; set; }
        [InverseProperty("Sender")]
        public ICollection<Message> MessagesSent { get; set; }

        [InverseProperty("Receiver")]
        public ICollection<Message> MessagesReceived { get; set; }
        // end of my code
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // my code
        //public System.Data.Entity.DbSet<Project2.Models.ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Message> Messages { get; set; }

        // end of my code
    }}
