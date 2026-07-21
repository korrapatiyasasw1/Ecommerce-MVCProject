using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface ICustomerAddressService
    {
        Task<List<CustomerAddress>> GetCustomerAddressById(int UserId);

        Task AddCustomerAddress(CustomerAddress customerAddress,int UserId);
        Task<CustomerAddress> UpdateAddress (int  customerAddressId);
        Task UpdateCustomerAddress(CustomerAddress customerAddress);
    }
}
