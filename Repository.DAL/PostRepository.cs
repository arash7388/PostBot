using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using Repository.Data;
using Repository.Data.Migrations;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class PostRepository : BaseRepository<Post>
    {
        public List<PostHelper> GetUserPosts(int userId)
        {
            IQueryable<PostHelper> res = from p in MTOContext.Posts
                                         join u in MTOContext.Users on p.UserId equals u.Id
                                         orderby p.InsertDateTime descending
                                         select new PostHelper()
                                             {
                                                 Id = p.Id,
                                                 Code = p.Code,
                                                 Context = p.Context,
                                                 Image = p.Image,
                                                 InsertDateTime = p.InsertDateTime,
                                                 Title = p.Title,
                                                 UserId = p.UserId,
                                                 FriendlyName = u.FriendlyName,
                                                 Tags = (from tp in MTOContext.TagPosts join t in MTOContext.Tags on tp.TagId equals t.Id where tp.PostId == p.Id select t).ToList()
                                             };

            return res.ToList();
        }

        public List<PostHelper> GetAllWithUserName()
        {
            IQueryable<PostHelper> res = from p in MTOContext.Posts
                                         join u in MTOContext.Users on p.UserId equals u.Id
                                         orderby p.InsertDateTime descending
                                         select new PostHelper()
                                         {
                                             Id = p.Id,
                                             Code = p.Code,
                                             Context = p.Context,
                                             Image = p.Image,
                                             InsertDateTime = p.InsertDateTime,
                                             Title = p.Title,
                                             UserId = p.UserId,
                                             FriendlyName = u.FriendlyName,
                                             Tags = (from tp in MTOContext.TagPosts join t in MTOContext.Tags on tp.TagId equals t.Id where tp.PostId == p.Id select t).ToList()
                                         };

            return res.ToList();
        }
        public List<PostHelper> GetPostsByTagId(int tagId, int count = 0)
        {
            var res = from p in MTOContext.Posts
                      join u in MTOContext.Users on p.UserId equals u.Id
                      join tp in MTOContext.TagPosts on p.Id equals tp.PostId
                      where tp.TagId == tagId
                      orderby p.InsertDateTime descending
                      select new PostHelper()
                                         {
                                             Id = p.Id,
                                             Code = p.Code,
                                             Context = p.Context,
                                             Image = p.Image,
                                             InsertDateTime = p.InsertDateTime,
                                             Title = p.Title,
                                             UserId = p.UserId,
                                             FriendlyName = u.FriendlyName,
                                             Tags = (from tps in MTOContext.TagPosts join t in MTOContext.Tags on tps.TagId equals t.Id where tps.PostId == p.Id select t).ToList()
                                         };

            if (count != 0)
                res = res.Take(count);
            
            return res.ToList();

        }

        /// <summary>
        /// Because repeater darasource needs list source we return a list instead of single entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PostHelper> GetPostById(int id)
        {
            var res = from p in MTOContext.Posts
                      join u in MTOContext.Users on p.UserId equals u.Id
                      where p.Id == id
                      orderby p.InsertDateTime descending
                      select new PostHelper()
                      {
                          Id = p.Id,
                          Code = p.Code,
                          Context = p.Context,
                          Image = p.Image,
                          InsertDateTime = p.InsertDateTime,
                          Title = p.Title,
                          UserId = p.UserId,
                          FriendlyName = u.FriendlyName,
                          Tags = (from tps in MTOContext.TagPosts join t in MTOContext.Tags on tps.TagId equals t.Id where tps.PostId == p.Id select t).ToList()
                      };
            
            return res.ToList();
        }

        public List<KeyValuePair<int,string>> Get10LatestPostsTitles()
        {
            return GetNLatestPostsTitles(10);
        }

        public List<KeyValuePair<int, string>> GetNLatestPostsTitles(int n)
        {
            var posts = (from p in MTOContext.Posts
                         orderby p.InsertDateTime descending
                         select new { p.Id, p.Title }).Take(n);

            var result = new List<KeyValuePair<int, string>>();

            foreach (var item in posts)
                result.Add(new KeyValuePair<int, string>(item.Id, item.Title));

            return result;
        }

        public List<Post> GetNLatestPosts(int n)
        {
            var posts = (from p in MTOContext.Posts
                         orderby p.InsertDateTime descending
                         select p).Take(n);

            return posts.ToList();
        }

        public List<KeyValuePair<string, int>> GetPostsGroupByMonthList()
        {
            var allPostsGroupByMonth = from p in MTOContext.Posts
                              group p by new { p.InsertDateTime.Value.Year , p.InsertDateTime.Value.Month}
                                  into g
                                  select new
                                  {
                                      Year = g.Key.Year,
                                      Month =  g.Key.Month,
                                      Count = g.Count(),
                                  };

            var result = new List<KeyValuePair<string, int>>();

            foreach (var item in allPostsGroupByMonth)
            {
                result.Add(new KeyValuePair<string, int>(item.Year + " " + item.Month, item.Count));
            }

            return result;
        }

        public List<PostHelper> GetPostByYearMonth(int year, int month)
        {
            var res = from p in MTOContext.Posts
                      join u in MTOContext.Users on p.UserId equals u.Id
                      where p.InsertDateTime.Value.Year == year && p.InsertDateTime.Value.Month==month
                      orderby p.InsertDateTime descending
                      select new PostHelper()
                      {
                          Id = p.Id,
                          Code = p.Code,
                          Context = p.Context,
                          Image = p.Image,
                          InsertDateTime = p.InsertDateTime,
                          Title = p.Title,
                          UserId = p.UserId,
                          FriendlyName = u.FriendlyName,
                          Tags = (from tps in MTOContext.TagPosts join t in MTOContext.Tags on tps.TagId equals t.Id where tps.PostId == p.Id select t).ToList()
                      };

            return res.ToList();
        }

        public List<PostHelper> GetPostsByFilter(string keyword)
        {
            var res = from p in MTOContext.Posts
                      join u in MTOContext.Users on p.UserId equals u.Id
                      where p.Title.ToLower().Contains(keyword.ToLower()) || p.Context.ToLower().Contains(keyword.ToLower())
                      orderby p.InsertDateTime descending
                      select new PostHelper()
                      {
                          Id = p.Id,
                          Code = p.Code,
                          Context = p.Context,
                          Image = p.Image,
                          InsertDateTime = p.InsertDateTime,
                          Title = p.Title,
                          UserId = p.UserId,
                          FriendlyName = u.FriendlyName,
                          Tags = (from tps in MTOContext.TagPosts join t in MTOContext.Tags on tps.TagId equals t.Id where tps.PostId == p.Id select t).ToList()
                      };

            return res.ToList();
        }

        public List<KeyValuePair<int, int>> GetPostsGroupByWriter()
        {
            var allPostsGroupByWriter = from p in MTOContext.Posts
                                        group p by  p.UserId 
                                           into g
                                           select new
                                           {
                                               UerId = g.Key,
                                               Count = g.Count(),
                                           };

            var result = new List<KeyValuePair<int, int>>();

            foreach (var item in allPostsGroupByWriter)
            {
                result.Add(new KeyValuePair<int, int>(item.UerId, item.Count));
            }

            return result;
        }

        public List<PostHelper> GetPostsByUserId(int userId, int count = 0)
        {
            var res = from p in MTOContext.Posts
                      join u in MTOContext.Users on p.UserId equals u.Id
                      //join tp in MTOContext.TagPosts on p.Id equals tp.PostId
                      where p.UserId == userId
                      orderby p.InsertDateTime descending
                      select new PostHelper()
                      {
                          Id = p.Id,
                          Code = p.Code,
                          Context = p.Context,
                          Image = p.Image,
                          InsertDateTime = p.InsertDateTime,
                          Title = p.Title,
                          UserId = p.UserId,
                          FriendlyName = u.FriendlyName,
                          Tags = (from tps in MTOContext.TagPosts join t in MTOContext.Tags on tps.TagId equals t.Id where tps.PostId == p.Id select t).ToList()
                      };

            if (count != 0)
                res = res.Take(count);

            return res.ToList();

        }
    }
    
    public class PostHelper : Post
    {
        public string FriendlyName { get; set; }
        public virtual List<Tag> Tags { get; set; }
    }
}
