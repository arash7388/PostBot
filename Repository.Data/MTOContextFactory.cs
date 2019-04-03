namespace Repository.Data
{
    public class MTOContextFactory : IMTOContextFactory
    {
        private readonly MTOContext _mtoContext;

        public MTOContextFactory()
        {
            this._mtoContext = new MTOContext();
        }

        public MTOContext GetMTOContext()
        {
            return this._mtoContext;
        }
    }
}
