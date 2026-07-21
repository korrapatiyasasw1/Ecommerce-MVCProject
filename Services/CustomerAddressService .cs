using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace MVCDotnetCore.Services
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly AppDbContext _context;
        
        public CustomerAddressService(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<List<CustomerAddress>> GetCustomerAddressById(int UserId)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == UserId);
            if(customer==null)
            {
                return null;
            }
            var customerAddresses = await _context.CustomerAddresses
                .Where(x => x.CustomerId == customer.Id).ToListAsync();
            if(!customerAddresses.Any())
            {
                return null;
            }
            return customerAddresses;
            

        }
        public async Task AddCustomerAddress(CustomerAddress customerAddress,int UserId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (customer == null)
            {
                throw new Exception("Customer not found ");
            } 
               
                    var customeraddress = new CustomerAddress()
                    {
                        CustomerId = customer.Id,
                        Street = customerAddress.Street,
                        City = customerAddress.City,
                        PostalCode = customerAddress.PostalCode,
                        Country = customerAddress.Country,
                        State = customerAddress.State,
                        HouseNo = customerAddress.HouseNo
                    };

                    _context.CustomerAddresses.Add(customeraddress);
                    await _context.SaveChangesAsync();
            
        }
        public async Task<CustomerAddress> UpdateAddress(int customerAddressId)
        {
           var address = _context.CustomerAddresses.
                FirstOrDefault(x=>x.Id ==  customerAddressId);
            if (address == null)
            {
                return null;
            }
            return address;
        }

        public async Task UpdateCustomerAddress(CustomerAddress customerAddress)
        {
            var a = _context.CustomerAddresses.FirstOrDefault();
            a.CustomerId = customerAddress.CustomerId;
            a.State = customerAddress.State;
            a.Street = customerAddress.Street;
            a.City = customerAddress.City;
            a.Country = customerAddress.Country;
            a.PostalCode = customerAddress.PostalCode;
            _context.CustomerAddresses.Update(a);
            await _context.SaveChangesAsync();
            
        }

    }
}
