using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
namespace MVCDotnetCore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Customer?> GetCustomerById(int userid)
        {
            Console.WriteLine($"{userid}");

            var customer = _context.Customers.FirstOrDefault(x => x.UserId == userid);
            var customer1 = _context.Customers.ToList();

            foreach(var i in customer1)
            {
                Console.WriteLine($"{i.Id} and {i.UserId}");
            }
           
            return customer;
                
        }
        public async Task AddCustomer(Customer customer ,int userId)
        {
            customer.UserId = userId;
          _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCustomer(Customer customer, int userId)
        {
            var cu= await _context.Customers.FirstOrDefaultAsync(X=>X.UserId == userId);
            if (cu == null)
            {
                throw new Exception("please enter your details");

            }
            cu.PhoneNumber = customer.PhoneNumber;
            cu.FirstName = customer.FirstName;
            cu.LastName = customer.LastName;
            cu.Email = customer.Email;
            cu.Address = customer.Address;
            await _context.SaveChangesAsync();
            
        }

    }
}
