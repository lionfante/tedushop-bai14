namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix_status_of_ContactDetails : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContactDetails", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContactDetails", "Status", c => c.String());
        }
    }
}
