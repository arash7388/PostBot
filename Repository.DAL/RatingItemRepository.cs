using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class RatingItemRepository:BaseRepository<RatingItem>
    {
        public List<RatingItem> GetByGroupId(int groupId)
        {
            return DbSet.Where(a => a.RatingGroupId == groupId).ToList();
        }
    }
}
