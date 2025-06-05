using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithCategoryAndBrandSpecifications : BaseSpecifications<Product>
    {
        /*This constructor will be used to create an Object, That will be used to Get All*/
        public ProductWithCategoryAndBrandSpecifications(ProductSpecParams specParams)
            : base(p => 
            (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) && 
            (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value) )
        {
            AddIncludes();
            if(!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
            
            ApplayPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        }

        /*This constructor will be used to create an Object, That will be used to Get specific product with id */
        public ProductWithCategoryAndBrandSpecifications(int id) : base(p => p.Id == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
    }
}
