using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrand();
       
    }
}
