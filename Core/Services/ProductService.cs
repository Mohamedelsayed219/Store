using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Abstractions;
using Services.Specifications;
using Shared;

namespace Services
{
    public class ProductService(IUnitofwork unitofwork, IMapper mapper) : IProductService
    {
        
        public async Task<PaginationResponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParamters specParams)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(specParams);

            // Get All Product Throught ProductRepository
            var products = await unitofwork.GetRepository<Product, int>().GetAllAsync(spec);


            var specCount = new ProductWithCountSpecifications(specParams);

            var count = await unitofwork.GetRepository<Product, int>().CountAsync(specCount);

            //var count = products.Count();

            // Mapping IEnumerable<Product> To IEnumerable<ProductResultDto>  : Automapper
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);


            return new PaginationResponse<ProductResultDto>(specParams.PageIndex,specParams.PageSize,count,result);


        }

        public async Task<ProductResultDto?> GetProductByTdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(id);

            var product = await unitofwork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundExceptions(id);

            var result = mapper.Map<ProductResultDto>(product);
            return result;

        }


        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitofwork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result;
        }

     

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await unitofwork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }

        
    }
}
