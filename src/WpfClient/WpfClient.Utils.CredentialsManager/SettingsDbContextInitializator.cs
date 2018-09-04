using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Utils.CredentialsManager
{
    internal class SettingsDbContextInitializator : SqliteCreateDatabaseIfNotExists<SettingsDbContext>
    {
        public SettingsDbContextInitializator(DbModelBuilder modelBuilder)
            : base(modelBuilder)
        {

        }
        protected override void Seed(SettingsDbContext context)
        {
            base.Seed(context);
        }
    }
}
