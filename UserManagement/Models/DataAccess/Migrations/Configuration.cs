namespace UserManagement.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UserManagement.Models.DataAccess;

    public sealed class Configuration : DbMigrationsConfiguration<DataEntities>
    {
            public Configuration()
            {
                AutomaticMigrationsEnabled = true;
            }

            protected override void Seed(DataEntities context)
            {
                RoleSeed.SeedRoles(context);
            }
    }
}
