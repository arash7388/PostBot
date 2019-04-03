using Common;

namespace Repository.DAL
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        ActionResult SaveChanges();
        void RejectChanges();

    }
}
