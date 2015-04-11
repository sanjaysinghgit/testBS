using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MLM.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using MLM.Web.Models;
using MLM.Models.Config;

namespace MLM.DB
{
    public class MLMDbContext : IdentityDbContext<ApplicationUser>
    {

        //public MLMDbContext()
        //{
        //    //todo: check this
        //    //this.Configuration.LazyLoadingEnabled = false;
        //    //this.Configuration.ProxyCreationEnabled = false;
        //}

        public MLMDbContext(string connectionString)
            : base(connectionString)
        {
            //todo: check this
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }

        public MLMDbContext(IConnectionRetriever connRetriever)
            : this(connRetriever.GetConnectionStringName())
        {

        }
        public string GetSchemaName()
        {
            return "dbo";
            //return "CustomModuleName";
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Properties().Where(p => p.Name.Equals("Id")).Configure(p => p.IsKey().HasColumnName(TableColumnNamesHelper.GetColumnNamePrependedByEntityName(p.ClrPropertyInfo)));

            //modelBuilder.Properties().Where(p => p.Name.Equals("RowVersion")).Configure(p => p.IsRowVersion().HasColumnName("RowVersion"));

            //modelBuilder.Types().Configure(t => t.ToTable(TableColumnNamesHelper.GetTableName(t.ClrType)));
            //modelBuilder.HasDefaultSchema(GetSchemaName());
            //AddConfigurations(modelBuilder);
            //base.OnModelCreating(modelBuilder);



            //modelBuilder.Entity<Agent>().MapToStoredProcedures();

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            // Change the name of the table to be Users instead of AspNetUsers
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");

        }

        //private void AddConfigurations(DbModelBuilder modelBuilder)
        //{
        //    var entityConfigurationTypes = FindTypesToRegister();
        //    entityConfigurationTypes.ToList().ForEach(entityConfigurationType =>
        //    {
        //        dynamic configurationInstance = Activator.CreateInstance(entityConfigurationType);
        //        modelBuilder.Configurations.Add(configurationInstance);
        //    });

        //}

        //private IEnumerable<Type> FindTypesToRegister()
        //{
        //    //TODO: More robust mechanism to discover the types
        //    var assembly = GetType().Assembly;
        //    var typesToRegister = assembly.GetTypes()
        //    .Where(type => type.Namespace != null && type.Namespace.Equals(GetType().Namespace))
        //    .Where(type => type.BaseType.IsGenericType &&
        //                   (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)
        //                    || type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityConfiguration<>)));
        //    return typesToRegister;
        //}

        ///// <summary>
        ///// A work around method to ensure EntityFramework.SqlServer assembly gets copied to the output.
        ///// </summary>
        //private void DummyMethod()
        //{

        //    var dummy = SqlProviderServices.Instance;
        //}

        # region "Products"
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategorys { get; set; }
        public DbSet<ProductGrade> ProductGrades { get; set; }
        public DbSet<ProductKit> ProductKits { get; set; }
        #endregion

        #region "Config"
        public DbSet<Setting> Settings { get; set; }
        #endregion

        # region "Agents"
        public DbSet<Agent> Agents { get; set; }
        public DbSet<EPin> EPins { get; set; }
        #endregion

        # region "Coupon"
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponType> CouponTypes { get; set; }
        #endregion

        #region "TempModels"

        public DbSet<TempAgTree> TempAgTrees { get; set; }

        #endregion

        // public DbSet<Student> Students { get; set; }
        
        
    }
}