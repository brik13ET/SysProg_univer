using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Runtime.Remoting.Contexts;

namespace DB_test
{
    public class DBE_Context : DbContext
    {
        public DBE_Context(string str = "DatabaseConnecionString") : base(str)
        {

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 5;
        }

        public DbSet<Main> records { get; set; }
    }
}
