using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCDotnetCore.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService ProductService;
        private readonly ICategoryService CategoryService;
        private readonly IBrandService brandService;
        private async Task LoadCategory()
        {
            ViewBag.Categories = await CategoryService.GetCategory();
        }
        private async Task LoadBrand()
        {
            ViewBag.Brands = await brandService.GetBrand();

        }
        public ProductController(IProductService _ProductService, 
            ICategoryService _CategoryService,IBrandService _brandService)
        {
            ProductService = _ProductService;
            CategoryService = _CategoryService;
            brandService = _brandService;

        }

        public async Task<IActionResult> Index()
        {
            await LoadCategory();
            var product = await ProductService.GetAllProducts();
           
            return View(product);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Categories = await CategoryService.GetCategory();
            ViewBag.Brands = await brandService.GetBrand();
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine(error.Key);

                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine(e.ErrorMessage);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                await ProductService.AddProduct(product);
                return RedirectToAction("Index", "Product");
            }
            return View(product);

        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await ProductService.GetProductById(id);

            if(product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await CategoryService.GetCategory();
            ViewBag.Brands = await brandService.GetBrand();

            return View("UpdateProduct", product);

        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
                {
                    await ProductService.UpdateProduct(product);
                    return RedirectToAction("Index");
                }
                return View();
            
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async void DeleteProduct(int id)
        {
                await ProductService.DeleteProduct(id);
        }
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> Search(string ProductName)
        {
           
                
               var product =  await ProductService.Search(ProductName);
            await LoadCategory();
            if (!product.Any())
            {
                return View("Error");
            }
            else
            {
                return View("Index", product);
            }
            
           
        }
        [Authorize(Roles = "Customer")]

        [HttpGet]
        public async Task<IActionResult> SearchByCategoryName(string CategoryName)
        {
            var product = await ProductService.SearchByCategoryName(CategoryName);
            await LoadCategory();
            //await LoadBrand();
            return View("Index",product);
        }
        [Authorize(Roles = "Customer")]

        [HttpGet]
       public async Task<IActionResult> ProductDetails(int id)
        {
          
            var product = await ProductService.GetProductById(id);
            if(product  == null)
            {
                return RedirectToAction("Index","Product");   
            }
            return View(product);
        }
    }
}

