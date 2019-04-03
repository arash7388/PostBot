using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class RatingItem:BaseEntity
    {
        public RatingGroup RatingGroup { get; set; }
        public int RatingGroupId { get; set; }
        public decimal Value { get; set; }
    }
}
