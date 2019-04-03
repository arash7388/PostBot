using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class RatingGroup:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
