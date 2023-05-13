using System;
using System.Data.Entity;

namespace SysProg_univer
{
    public class DBE_Context : DbContext
    {
        public DBE_Context(string str = "DatabaseConnecionString") : base(str)
        {

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 5;
        }

        public DbSet<DbRecord> records { get; set; }
    }
}
