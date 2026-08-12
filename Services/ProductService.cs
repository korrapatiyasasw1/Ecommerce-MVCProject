using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;
using MVCDotnetCore.DTOs;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Diagnostics;

namespace MVCDotnetCore.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGenericRepository<Product> _repository;

        public ProductService(AppDbContext context,
                         IWebHostEnvironment webHostEnvironment, 
                         IGenericRepository<Product> repository)

        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<List<ProductDto>> GetAllProducts()
        {
           return await _context.Products.Include(x => x.Category).Include(x => x.Brand)
                      .Select(X => new ProductDto
                      {
                          Id = X.Id,
                          Name = X.Name,
                          Price = X.Price,
                          ImageUrl = X.ImageUrl,
                          CategoryName = X.Category.Name,
                          BrandName = X.Brand.BrandName
                      }).ToListAsync();
        }


       public async Task AddProduct(Product product)
        {
            if (product.ImageFile != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath,
                                             "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(product.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
        }
        public async Task<Product> GetProductByIdForEdit(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new Exception("product is not null");
            }
            return product;
        }
        public async Task<ProductDetailsDTo> GetProductById(int id)
        {
            var product = await _context.Products.Include(x => x.Category).Include(x => x.Brand)
                      .Select(X => new ProductDetailsDTo
                      {
                          Id = X.Id,
                          Name = X.Name,
                          Price = X.Price,
                          ImageUrl = X.ImageUrl,
                          CategoryName = X.Category.Name,
                          BrandName = X.Brand.BrandName,
                          Description = X.Description,
                          BrandDescription = X.Brand.Description

                      }).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new Exception("product is not null");
            }
            return product;
        }
        public async Task UpdateProduct(Product product)
        {
            var prod = await _context.Products.
                FirstOrDefaultAsync(c => c.Id == product.Id);

            if (prod==null)
            {
                throw new Exception("product not there");
            }
            prod.Name = product.Name;
            prod.Description = product.Description;
            prod.Price = product.Price;
            prod.stock = product.stock;
            prod.CategoryId = product.CategoryId;
            prod.BrandId = product.BrandId;
            prod.IsAcitve = product.IsAcitve;
            if (product.ImageFile != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(product.ImageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                prod.ImageUrl = "/images/" + fileName;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int Id)
        {
            var prod = _context.Products.FirstOrDefault(x=>x.Id==Id);
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();  

        }
        public async Task<List<ProductDto>> Search(string ProductName)
        {
            if (ProductName == null)
            {

                return await _context.Products.Include(x => x.Category).Include(x => x.Brand)
                  .Select(X => new ProductDto
                  {
                      Id = X.Id,
                      Name = X.Name,
                      Price = X.Price,
                      ImageUrl = X.ImageUrl,
                      CategoryName = X.Category.Name,
                      BrandName = X.Brand.BrandName
                  }).ToListAsync();
            }
            else
            {
                return await _context.Products.
                    Include(x => x.Category).
                    Include(x => x.Brand)
                    .Select(X => new ProductDto
             {
                 Id = X.Id,
                 Name = X.Name,
                 Price = X.Price,
                 ImageUrl = X.ImageUrl,
                 CategoryName = X.Category.Name,
                 BrandName = X.Brand.BrandName
             }).Where(x => x.Name.Contains(ProductName)).
             ToListAsync();
            } 
         
        }
       public async  Task<List<ProductDto>> SearchByCategoryName(string CategoryName)
        {
            if (CategoryName == null)
            {
                return await _context.Products.
                    Include(x => x.Category).
                    Include(x => x.Brand)
                  .Select(X => new ProductDto
                  {
                      Id = X.Id,
                      Name = X.Name,
                      Price = X.Price,
                      ImageUrl = X.ImageUrl,
                      CategoryName = X.Category.Name,
                      BrandName = X.Brand.BrandName
                  }).ToListAsync();
            }
           return await _context.Products.
                Include(x => x.Category).
                Include(x => x.Brand)
             .Select(X => new ProductDto
             {
                 Id = X.Id,
                 Name = X.Name,
                 Price = X.Price,
                 ImageUrl = X.ImageUrl,
                 CategoryName = X.Category.Name,
                 BrandName = X.Brand.BrandName
             }).Where(x => x.CategoryName
             .Contains(CategoryName)).
             ToListAsync();

        }

    }
}
