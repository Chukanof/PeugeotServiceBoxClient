using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Utils.CredentialsManager
{
    public class SettingsDbContext : DbContext
    {
        public SettingsDbContext()
            : base(nameof(SettingsDbContext)) {
        }
        public virtual DbSet<CredentialEntry> Credentials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var initializer = new SettingsDbContextInitializator(modelBuilder);
            Database.SetInitializer(initializer);


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CredentialEntry>()
                .HasIndex(e => e.Login)
                .IsUnique();
        }
    }
}
