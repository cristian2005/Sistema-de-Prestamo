﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sistema_de_Prestamo.Startup))]
namespace Sistema_de_Prestamo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
