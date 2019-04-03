using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Data;
using Repository.Entity.Domain;

namespace Repository.DAL
{
    public class ProductRepository:BaseRepository<Product>
    {
        public List<Product> GetByCatId(int catId)
        {
            return DbSet.Where(a => a.ProductCategoryId == catId).ToList();
        }

        public List<ProductHelper> GeAllWithCatName()
        {
            var r = from p in MTOContext.Products
                    join c in MTOContext.ProductCategories.DefaultIfEmpty() on p.ProductCategoryId equals c.Id
                    select new ProductHelper
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                        InsertDateTime = p.InsertDateTime,
                        Status = p.Status,
                        CategoryName = c.Name,
                        ProductCategoryId = p.ProductCategoryId,
                        Image = p.Image
                    };

            return r.ToList();
        }

        public List<ProductHelper> GetByCatIdWithCatName(int catId)
        {
            var r = from p in MTOContext.Products
                    join c in MTOContext.ProductCategories.DefaultIfEmpty() on p.ProductCategoryId equals c.Id
                where p.ProductCategoryId==catId
                select new ProductHelper
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    InsertDateTime = p.InsertDateTime,
                    Status = p.Status,
                    CategoryName = c.Name,
                    ProductCategoryId = p.ProductCategoryId,
                    Image = p.Image
                };

            return r.ToList();
        }



    }

    public class ProductHelper : Product
    {
        public string CategoryName { get; set; }
    }
}
