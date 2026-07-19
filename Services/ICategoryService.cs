using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategory();
    }
}
