using System.Data.Entity.ModelConfiguration;
using Repository.Entity.Domain;

namespace Repository.Entity.Map
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
        //     modelBuilder.Entity<Artist>()
        //Property(a=>a.).HasMany(c => c.p)
        //.WithMany(x => x.Artists)
        //.Map(a => {
        //    a.ToTable("ArtistsGenres");
        //    a.MapLeftKey("ArtistId");
        //    a.MapRightKey("GenreId");
        //});

            //HasKey(a=>a.Id);
            //Property(a => a.Title).IsRequired().HasMaxLength(50);
            //Property(a => a.Code).IsRequired().HasMaxLength(50);
            //Property(a => a.PayShowableTitle).HasMaxLength(50);
            //Property(a => a.RowGuid).IsRequired();
            //HasRequired(a => a.PegahUserInserted).WithMany()
            //    .HasForeignKey(u => u.PegahUserInsertedGuid)
            //    .WillCascadeOnDelete(false);
            //Property(a => a.EffectiveDate).IsRequired().HasMaxLength(10).IsFixedLength().IsUnicode(false);
            //ToTable("SalaryItems");
        }
    }
}
