using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
   
    public class Post:BaseEntity
    {
        [DisplayName("کد")]
        public string Code { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("محتوا")]
        public string Context { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        [DisplayName("تصویر")]
        public byte[] Image { get; set; }

        public virtual ICollection<TagPost> TagPosts { get; set; }

        [DisplayName("خلاصه")]
        public string BriefContext { get; set; }

    }
}
