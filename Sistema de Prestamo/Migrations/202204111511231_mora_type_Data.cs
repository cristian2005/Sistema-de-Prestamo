namespace Sistema_de_Prestamo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mora_type_Data : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Moras", "Pagado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Moras", "Pagado", c => c.Int(nullable: false));
        }
    }
}
