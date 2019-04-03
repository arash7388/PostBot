using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class TagPost:BaseEntity
    {
        public Tag Tag { get; set; }
        public int TagId { get; set; }

        public Post Post { get; set; }
        public int PostId { get; set; }
    }
}
