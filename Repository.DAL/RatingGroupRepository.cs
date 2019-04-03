using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class RatingGroupRepository:BaseRepository<RatingGroup>
    {
        public decimal GetRatingGroupCurrentValue(int id)
        {
            decimal avgValue = 0;

            var relatedItems = new RatingItemRepository().GetByGroupId(id);

            if (relatedItems != null && relatedItems.Any())
                avgValue = relatedItems.Average(a => a.Value);

            return avgValue;
        }
    }
}
