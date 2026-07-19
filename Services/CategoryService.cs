using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategory()
        {
            var category = await _context.Categories.ToListAsync();
            return category;
        }

    }
}
