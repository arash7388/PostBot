using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class Product:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        
        [DisplayName("تصویر")]
        public byte[] Image { get; set; }
    }
}
