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
            
            CreateTable(
                "dbo.Prestamos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Monto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Interes = c.Int(nullable: false),
                        NoCuotas = c.Int(nullable: false),
                        FormaPago = c.String(nullable: false),
                        FechaInicio = c.DateTime(nullable: false),
                        MontoCuota = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalIntereses = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MontoPagar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cliente_Id = c.Int(nullable: false),
                        Prestadore_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.Cliente_Id, cascadeDelete: true)
                .ForeignKey("dbo.Prestadores", t => t.Prestadore_Id, cascadeDelete: true)
                .Index(t => t.Cliente_Id)
                .Index(t => t.Prestadore_Id);
            
            CreateTable(
                "dbo.Cuotas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        noCuota = c.Int(nullable: false),
                        fecha_pago = c.String(),
                        interes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        capital = c.Decimal(nullable: false, precision: 18, scale: 2),
                        restante = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Prestamo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prestamos", t => t.Prestamo_Id, cascadeDelete: true)
                .Index(t => t.Prestamo_Id);
            
            CreateTable(
                "dbo.Prestadores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Cedula = c.String(nullable: false),
                        Direccion = c.String(),
                        Telefono = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Prestamos", "Prestadore_Id", "dbo.Prestadores");
            DropForeignKey("dbo.Cuotas", "Prestamo_Id", "dbo.Prestamos");
            DropForeignKey("dbo.Prestamos", "Cliente_Id", "dbo.Clientes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Cuotas", new[] { "Prestamo_Id" });
            DropIndex("dbo.Prestamos", new[] { "Prestadore_Id" });
            DropIndex("dbo.Prestamos", new[] { "Cliente_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Prestadores");
            DropTable("dbo.Cuotas");
            DropTable("dbo.Prestamos");
            DropTable("dbo.Clientes");
        }
    }
}
