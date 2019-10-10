
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using sellmarket.Models;

namespace sellmarket.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MarketContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    } 
}