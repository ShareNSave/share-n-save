using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using ZeroWaste.SharePortal.Migrations;
using ZeroWaste.SharePortal.Models.Data;
using Lucene.Net.Support;

namespace ZeroWaste.SharePortal
{
    public static class MigrationsConfig
    {
        public static void Init()
        {
            var runMigrationsSetting = ConfigurationManager.AppSettings["site:RunMigrations"];
            bool runMigrations = string.Compare(runMigrationsSetting, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0;
            if (runMigrations)
            {
                var dataLossSetting = ConfigurationManager.AppSettings["site:DataLossAllowed"];
                bool dataLossAllowed = string.Compare(dataLossSetting, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0;

                var configuration = new DbMigrationsConfiguration
                {
                    AutomaticMigrationsEnabled = true,
                    AutomaticMigrationDataLossAllowed = dataLossAllowed,
                    ContextType = typeof(ZeroWasteData),
                    TargetDatabase = new DbConnectionInfo("DefaultConnection"),
                    MigrationsAssembly = typeof(Initial).Assembly
                };

                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
        }
    }
}