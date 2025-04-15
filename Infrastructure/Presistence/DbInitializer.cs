using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;

namespace Presistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbInitializer(StoreDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            
            
                // Craete Database If It dosen't Exists && Apply To Any Pending Migrations
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }



                // Data Seding 

                // Seeding ProductType From Json Files

                if (!_context.ProductTypes.Any())
                {

                    // 1. Read All Data From Types Json File as String

                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\types.json");


                    // 2. Transform String To C# Object [List<ProductTypes>]
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);


                    // 3. Add List<ProductTypes> To Database

                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }

                }


                // Seeding ProductBrands From Json Files

                if (!_context.ProductBrands.Any())
                {

                    // 1. Read All Data From brands Json File as String

                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\brands.json");


                    // 2. Transform String To C# Object [List<ProductBrands>]
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


                    // 3. Add List<ProductBrands> To Database

                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }

                }

                // Seeding Products From Json Files


                if (!_context.Products.Any())
                {

                    // 1. Read All Data From products Json File as String

                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Presistence\Data\Seeding\products.json");


                    // 2. Transform String To C# Object [List<Products>]
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                    // 3. Add List<Products> To Database

                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }

                }

            
            
          
            
         

        }


        // C:\Users\hp\source\repos\Store.G02\Infrastructure\Presistence\Data\Seeding\types.json
        // C:\Users\hp\source\repos\Store.G02\Infrastructure\Presistence\Data\Seeding\brands.json
        // C:\Users\hp\source\repos\Store.G02\Infrastructure\Presistence\Data\Seeding\products.json
    }
}
