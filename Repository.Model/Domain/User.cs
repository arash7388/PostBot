using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
   
    public class User:BaseEntity
    {
        [DisplayName("نام کاربری")]
        public string Username { get; set; }
        
        [DisplayName("کلمه عبور")]
        public string Password { get; set; }

        [DisplayName("نام مستعار")]
        public string FriendlyName { get; set; }

        [DisplayName("تصویر")]
        public byte[] Image { get; set; }
    }
}
