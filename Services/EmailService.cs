using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace MVCDotnetCore.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly PDFService pdfService;
        public EmailService(
            IOptions<EmailSettings> settings, PDFService pdfService)
        {
            _settings = settings.Value;
            this.pdfService = pdfService;
        }
        public async Task SendOrderStatusEmail(string email,
                                      string customerName,
                                      string orderNumber,
                                      string status)
        {

            var message = new MailMessage();
            message.From = new MailAddress(_settings.Email); ;
            message.To.Add(new MailAddress(email));
            message.Subject = "Hi ";
            message.Body = $@"
                           Hello {customerName},
                           Your order status has been updated
                           Order Number: {orderNumber}
                           Current Status: {status}
                           Thank you for shopping with us.";
            var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Email,
               _settings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
        public async Task SendOrderInvoiceMail(Order orders, byte[] pdf)
        {

            var message = new MailMessage();
            message.From = new MailAddress(_settings.Email);
            message.To.Add(new MailAddress(orders.Customer.User.Email));
            message.Subject = "Order Invoice";
            message.Attachments.Add(new Attachment(new MemoryStream(pdf),
                "Invoice.pdf", "application/pdf"));
            message.Body = $@"
                           Hello {orders.Customer.FirstName},
                           Please find attached the invoice for your order.
                           Order Number: {orders.OrderNumber}
                           Thank you for shopping with us.";
            var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Email,
               _settings.Password),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);


        }
        public async Task SendOrderConfirmationEmail(Order order)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_settings.Email);
            message.To.Add(new MailAddress(order.Customer.Email));
            message.Subject = "Your order has been raised ";
            Debug.WriteLine($"Sending order confirmation email to" +
                $"" +
                $" {order.Customer.Email}");

            message.Body = $@"hi 
        {order.Customer.LastName} 
            Your order has been created";
            var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Email,
               _settings.Password),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);

        }
        public async Task NewOrderhasbeenCreated(Order order)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_settings.Email);
            message.To.Add(new MailAddress(_settings.Email));
            message.Subject = "New Order Created";
            message.Body = $@" 
                order has been created with order number 
                               {order.OrderNumber} the mail 
                                     is {order.Customer.Email}";

            var smtp = new SmtpClient(_settings.Host,
                _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Email,
               _settings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
        }
    }
}

