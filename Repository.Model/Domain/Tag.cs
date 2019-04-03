using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    
    public class Tag:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TagPost> TagPosts { get; set; } 
    }
}
