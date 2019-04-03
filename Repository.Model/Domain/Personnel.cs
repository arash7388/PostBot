using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entity.Domain
{
    public class Personnel:BaseEntity
    {
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string NationalCode { get; set; }
    }
}
