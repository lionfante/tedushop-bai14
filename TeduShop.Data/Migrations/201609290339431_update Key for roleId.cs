namespace TeduShop.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateKeyforroleId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropPrimaryKey("dbo.ApplicationRoleGroups");
            AlterColumn("dbo.ApplicationRoleGroups", "RoleId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ApplicationRoleGroups", new[] { "GroupId", "RoleId" });
            CreateIndex("dbo.ApplicationRoleGroups", "RoleId");
            AddForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles");
            DropForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups");
            DropIndex("dbo.ApplicationRoleGroups", new[] { "RoleId" });
            DropPrimaryKey("dbo.ApplicationRoleGroups");
            AlterColumn("dbo.ApplicationRoleGroups", "RoleId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.ApplicationRoleGroups", "GroupId");
            CreateIndex("dbo.ApplicationRoleGroups", "RoleId");
            AddForeignKey("dbo.ApplicationRoleGroups", "RoleId", "dbo.ApplicationRoles", "Id");
            AddForeignKey("dbo.ApplicationRoleGroups", "GroupId", "dbo.ApplicationGroups", "ID");
        }
    }
}
