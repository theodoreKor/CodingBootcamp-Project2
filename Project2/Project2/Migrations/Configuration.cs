namespace Project2.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Project2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project2.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Project2.Models.ApplicationDbContext";
        }

        protected override void Seed(Project2.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // my code

            // add Roles

            // create Admin Role
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                context.Roles.AddOrUpdate(
                    r => r.Name,
                    new IdentityRole { Name = "Admin" }
                    );
            }

            // create User Role
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                context.Roles.AddOrUpdate(
                    r => r.Name,
                    new IdentityRole { Name = "User" }
                    );
            }

            // create God Role
            if (!context.Roles.Any(r => r.Name == "God"))
            {
                context.Roles.AddOrUpdate(
                    r => r.Name,
                    new IdentityRole { Name = "God" }
                    );
            }

            // add users

            // create Admin
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
    
                var user = new ApplicationUser { UserName = "Admin", Email = "admin@admin.com", Avatar = "/Content/Images/Avatars/Admin.jpg", Role = UserRoles.Admin };
                manager.Create(user, "12qw!@QW");
                manager.AddToRole(user.Id, "Admin");
            }

            // create God
            if (!context.Users.Any(u => u.UserName == "Goddess"))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
          
                var user = new ApplicationUser { UserName = "Goddess", Email = "goddess@goddess.com", Avatar = "/Content/Images/Avatars/god.jpg", Role = UserRoles.God };
                manager.Create(user, "12qw!@QW");
                manager.AddToRole(user.Id, "God");
            }

            // create ChatBot
            if (!context.Users.Any(u => u.UserName == "ChatBot"))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

                var user = new ApplicationUser { UserName = "ChatBot", Email = "chatbot@chatbot.com", Avatar = "/Content/Images/Avatars/chatbot.png", Role = UserRoles.User };
                manager.Create(user, "12qw!@QW");
                manager.AddToRole(user.Id, "User");
            }
            
            // end of my code
        }
    }
}
