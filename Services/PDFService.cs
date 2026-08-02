using MailKit.Search;
using MVCDotnetCore.Models;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Utilities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace MVCDotnetCore.Services
{
    public class PDFService
    {
        private readonly IOrderService orderService;
        public  byte[] GeneratepdfInvoice(Order order)
        {
            var pdf = Document.Create(x =>
            {
                x.Page(x =>
                {
                x.Content().Column(c=>
                {
                    c.Item().Text($"{order.OrderNumber}");
                    c.Item().Text($"{order.OrderDate}");
                    c.Item().Text($"{order.Customer.Email}");
                    var orderdate = DateTime.Now;
                    c.Item().Text($"orderdate");
                });
                });
                x.Page(x =>
                {
                    x.Content().Text("Hello");
                });
            } );
           
                return pdf.GeneratePdf();

        }
      

    }
}
