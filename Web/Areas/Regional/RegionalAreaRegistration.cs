﻿using System.Web.Mvc;

namespace Cats.Areas.Regional
{
    public class RegionalAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Regional";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Regional_default",
                "Regional/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                name: "Regional_Start",
                url: "Regional/",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "Cats.Areas.Regional.Controllers" }
                );
            
        }
    }
}