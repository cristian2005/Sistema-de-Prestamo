namespace Sistema_de_Prestamo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregadoCampoPagado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cuotas", "pagado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cuotas", "pagado");
        }
    }
}
