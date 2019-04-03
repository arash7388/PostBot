using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class Subscriber:BaseEntity
    {
        [EmailAddress(ErrorMessage = "آدرس ایمیل وارد شده صحیح نیست")]
        public string EMail { get; set; }

        public DateTime SubscribeDate { get; set; }
    }
}
