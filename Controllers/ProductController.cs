using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;

namespace MVCDotnetCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService ProductService;
        private readonly ICategoryService CategoryService;
        private async Task LoadCategory()
        {
            ViewBag.Categories = await CategoryService.GetCategory();
        }
        public ProductController(IProductService _ProductService, 
            ICategoryService _CategoryService)
        {
            ProductService = _ProductService;
            CategoryService = _CategoryService;

        }
        public async Task<IActionResult> Index()
        {
            var product = await ProductService.GetAllProducts();
            ViewBag.Categories = await CategoryService.GetCategory();

            return View(product);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
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
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var product = await ProductService.GetProductById(id);

            if(product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await CategoryService.GetCategory();
            return View("UpdateProduct", product);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
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
                    await ProductService.UpdateProduct(product);
                    return RedirectToAction("Index");
                }
                return View();
            
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)

        {
            
                await ProductService.DeleteProduct(id);

            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Search(string ProductName)
        {
            await LoadCategory();

            var product = await ProductService.Search(ProductName);
           
            return View("Index",product);
        }
        [HttpGet]
        public async Task<IActionResult> SearchByCategoryName(string CategoryName)
        {
             await LoadCategory();
            var product = await ProductService.SearchByCategoryName(CategoryName);
            return View("Index",product);
        }
        [HttpGet]
       public async Task<IActionResult> ProductDetails(int id)
        {
            await LoadCategory();
            var product = await ProductService.GetProductById(id);
            if(product  == null)
            {
                return RedirectToAction("Index");   
            }
            return View(product);
        }
    }
}

