using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class TagRepository : BaseRepository<Tag>
    {
        /// <summary>
        /// Tags left join TagPosts to get all tags + checked tags related to a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public List<TagHelper> GetAllTagsForPost(int postId)
        {
            IQueryable<TagHelper> rslt = null;
            try
            {
                var tags = from a in MTOContext.Tags select a;

                var tagPosts = MTOContext.TagPosts.Where(a => a.PostId == postId);

                rslt = from t in tags
                    join pt in tagPosts on t.Id equals pt.TagId
                        into temp
                    from s in temp.DefaultIfEmpty()
                    select new TagHelper()
                    {
                        Code = t.Code,
                        Name = t.Name,
                        TagId = t.Id,
                        PostId = s.PostId
                    };
            }
            catch (Exception ex)
            {
                //todo
            }

            return rslt==null ? null : rslt.ToList();
        }

        public class TagHelper
        {
            public int TagId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public int? PostId { get; set; }
        }
    }
}
