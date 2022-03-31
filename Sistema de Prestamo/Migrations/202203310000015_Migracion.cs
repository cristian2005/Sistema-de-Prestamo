namespace Sistema_de_Prestamo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Cedula = c.String(nullable: false),
                        Direccion = c.String(),
                        Telefono = c.String(),
                        Salario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clientes");
        }
    }
}
