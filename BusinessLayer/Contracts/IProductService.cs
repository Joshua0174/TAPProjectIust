using BusinessLayer.Dto.ProductDto;
using BusinessLayer.Helpers;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public interface IProductService
    {
        public List<ProductDto> GetAll(QueryObject query);
        public ProductDto GetById(Guid id);
        public ProductDto Create(Product productModel);
        public ProductDto Update(Guid id, Product productModel);
        public void Delete(Guid id);
    }
}
