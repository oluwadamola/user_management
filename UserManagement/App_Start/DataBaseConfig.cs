using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;
using UserManagement.Migrations;

namespace UserManagement.App_Start
{
    public class DataBaseConfig
    {
        public static void MigrateToLatest()
        {
            //upgrade db to latest
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
    }
}