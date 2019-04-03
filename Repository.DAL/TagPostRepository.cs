using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entity.Domain;

namespace Repository.DAL
{

    public class TagPostRepository : BaseRepository<TagPost>
    {
        public List<TagPostHelper> GetTagPostsGroupByTags()
        {
            var allTagPosts = from tp in MTOContext.TagPosts
                              group tp by tp.TagId
                                  into g
                                  select new
                                  {
                                      Id = g.Key,
                                      Count = g.Count(),
                                  };

            var allTags = from t in MTOContext.Tags select t;

            var res = from t in allTags
                      join r in allTagPosts
                      on t.Id equals r.Id
                      select new TagPostHelper()
                      {
                          TagId = t.Id,
                          TagName = t.Name,
                          Count = r.Count
                      };

            return res.ToList();
        }
    }

    public class TagPostHelper 
    {
        public int TagId { get; set; }
        public int Count { get; set; }
        public string TagName { get; set; }
    }
}
