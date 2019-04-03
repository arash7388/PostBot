using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{

    /// <summary>
    /// Data in DocumentType table has been hard coded in this order
    /// </summary>
    public enum DocumentTypeEnum
    {
        Inbox = 1,
        Sent = 2,
        Draft = 3,
        Deleted = 4,
        Board = 5,
        Request = 6

    }
}
