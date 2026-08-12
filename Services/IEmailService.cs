using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface IEmailService
    {
        Task SendOrderStatusEmail(string email,
                                      string customerName,
                                      string orderNumber,
                                      string status);
        Task SendOrderInvoiceMail(Order orders, byte[] pdf);
        Task SendOrderConfirmationEmail(Order order);
        Task NewOrderhasbeenCreated(Order order);
    }
}
