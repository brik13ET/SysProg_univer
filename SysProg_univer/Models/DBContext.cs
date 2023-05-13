using System;
using System.Data.Entity;

namespace SysProg_univer
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext(string str = "DatabaseConnecionString") : base(str)
        {

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 5;
        }

        public DbSet<Record> records { get; set; }
    }
}
