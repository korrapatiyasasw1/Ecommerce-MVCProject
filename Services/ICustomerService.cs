using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface ICustomerService
    {

       Task<Customer?> GetCustomerById(int userid);

        Task AddCustomer(Customer customer,int userId);
        Task UpdateCustomer(Customer customer, int userId);


       // Task DeleteCustomer(int id);
    }

}
