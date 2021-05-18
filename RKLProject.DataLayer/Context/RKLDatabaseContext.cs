using System.Linq;
using Microsoft.EntityFrameworkCore;
using RKLProject.Entities.Access;
using RKLProject.Entities.Client;
using RKLProject.Entities.Forms;
using RKLProject.Entities.Organization;
using RKLProject.Entities.User;

namespace RKLProject.DataLayer
{
    public class RKLDatabaseContext : DbContext
    {
        #region Constructor

        public RKLDatabaseContext(DbContextOptions<RKLDatabaseContext> options) : base(options) { }

        #endregion

        #region DBSets
        public DbSet<User> Users { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormDetail> FormDetails { get; set; }
        public DbSet<MasterForm> MasterForms { get; set; }
        public DbSet<MasterFormDetails> MasterFormDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }
        public DbSet<Client> Clients { get; set; }

        #endregion

        #region disable cascading delete in database

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var cascades = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascades)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }
}