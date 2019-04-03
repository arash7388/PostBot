using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entity.Domain;

namespace Repository.Entity.Map
{
    public class DocumentToMap:EntityTypeConfiguration<DocumentTo>
    {
        public DocumentToMap()
        {
            HasRequired(a => a.User).WithMany()
                .HasForeignKey(u => u.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Document).WithMany()
               .HasForeignKey(u => u.DocumentId)
               .WillCascadeOnDelete(false);
        }
    }
}
