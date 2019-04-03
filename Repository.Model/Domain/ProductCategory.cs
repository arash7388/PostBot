using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class ProductCategory:BaseEntity
    {
        [DisplayName("نام")]
        public string  Name { get; set; }
        public ProductCategory ProductCat { get; set; }
        
        [DisplayName("شناسه گروه والد")]
        public int? ProductCatId { get; set; }

        [DisplayName("تصویر")]
        public byte[] Image { get; set; }
    }
}
