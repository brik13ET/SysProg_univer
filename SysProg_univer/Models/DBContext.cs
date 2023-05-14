using System;
using System.Data.Entity;

namespace SysProgUniver
{
    public class DBContext : System.Data.Entity.DbContext
    {
        public DBContext(string str) : base(str)
        {

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 5;
        }
        public DBContext() : base("DatabaseConnecionString")
        {

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 5;
        }


        public DbSet<Record> Records { get; set; }
    }
}
