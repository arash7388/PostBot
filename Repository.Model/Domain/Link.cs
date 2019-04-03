using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class Link:BaseEntity
    {
        public string Title { get; set; }
        public string Href { get; set; }
    }
}
