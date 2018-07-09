namespace Project2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gfds : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "HasAvatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "HasAvatar", c => c.Boolean(nullable: false));
        }
    }
}
