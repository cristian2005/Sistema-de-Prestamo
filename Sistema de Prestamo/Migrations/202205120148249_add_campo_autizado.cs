namespace Sistema_de_Prestamo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_campo_autizado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cuotas", "autorizado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cuotas", "autorizado");
        }
    }
}
