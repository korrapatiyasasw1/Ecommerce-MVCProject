using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        public BrandService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Brand>> GetBrand()
        {
              var brand = await _context.Brands.ToListAsync();
            if(brand == null)
            {
                return null;
            }
            return brand;
        }

    }
}
