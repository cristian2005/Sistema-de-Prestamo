namespace Sistema_de_Prestamo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_mora : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Moras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MontoPagado = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaPago = c.DateTime(nullable: false),
                        Pagado = c.Int(nullable: false),
                        IdCuota = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cuotas", t => t.IdCuota, cascadeDelete: true)
                .Index(t => t.IdCuota);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moras", "IdCuota", "dbo.Cuotas");
            DropIndex("dbo.Moras", new[] { "IdCuota" });
            DropTable("dbo.Moras");
        }
    }
}
