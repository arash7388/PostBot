using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{
    using Repository.Entity.Domain;

    public class ProductCategoryRepository :BaseRepository<ProductCategory>
    {
        public List<ProductCategoryHelper> GetAllWithParents()
        {
            var result = from p in MTOContext.ProductCategories
                         join i in MTOContext.ProductCategories
                on p.ProductCat equals i into ps
                         from pp in ps.DefaultIfEmpty()
                select new ProductCategoryHelper
                {
                    Id = p.Id,
                    Name = p.Name,
                    InsertDateTime = p.InsertDateTime,
                    ParentName =  pp.Name,
                    Status = p.Status,
                    ProductCatId = p.ProductCatId,
                    UpdateDateTime = p.UpdateDateTime
                };

            return result.ToList();

        }

        public List<ProductCategory> GetFirstSixElements()
        {
            return MTOContext.ProductCategories.Take(6).ToList();
        }

        public List<ProductCategory> GetLastSixElements()
        {
            return MTOContext.ProductCategories.OrderByDescending(a=>a.InsertDateTime).Take(6).ToList();
        }
    }

    public class ProductCategoryHelper : ProductCategory
    {
        public string ParentName { get; set; }

        
    }
}
