using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using CodeFirstStoreFunctions;
using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;
using Repository.Data.Migrations;
using Repository.Entity.Domain;

namespace Repository.Data
{
    public class MTOContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<ProductCategory> ProductCategories { get; set; }
        //public DbSet<Subscriber> Subscribers { get; set; }
        //public DbSet<RatingGroup> RatingGroups { get; set; }
        public DbSet<RatingItem> RatingItems { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<TagPost> TagPosts { get; set; }
        public DbSet<Personnel> Personnels { get; set; }

        //public MTOContext() :base(System.Configuration.ConfigurationManager.ConnectionStrings["MTO"].ToString())
        //public MTOContext() :base("MTO")
        //public MTOContext(): base("Shali")
        public MTOContext() :base("Data Source=.;Initial Catalog=DotinBot;Integrated Security=SSPI;")
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MTOContext>());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MTOContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var asm = Assembly.GetExecutingAssembly();
            this.LoadEntity(asm, modelBuilder);
            //this.LoadEntityConfigurations(asm, modelBuilder);

            //modelBuilder.Configurations.Add(new DocumentToMap());

            modelBuilder.Conventions.Add(new FunctionsConvention<MTOContext>("dbo"));
            base.OnModelCreating(modelBuilder);
        }

        private void LoadEntity(Assembly asm, DbModelBuilder dbModelBuilder)
        {

            var entityTypes = asm.GetTypes()
                .Where(type => type.BaseType != null &&
                    //type.Namespace == "RepositoryPattern.Model.Domain" &&
                    //type.BaseType.IsAbstract &&
                               type.BaseType == typeof(BaseEntity))
                .ToList();

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");
            entityTypes.ForEach(type =>
                entityMethod.MakeGenericMethod(type).Invoke(dbModelBuilder, new object[] { }));
        }

        private void LoadEntityConfigurations(Assembly asm, DbModelBuilder dbModelBuilder)
        {

            var configurations = asm.GetTypes()
                .Where(type => type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
                .ToList();
            configurations.ForEach(type =>
            {
                dynamic instance = Activator.CreateInstance(type);
                dbModelBuilder.Configurations.Add(instance);
            });
        }

        
    }
}