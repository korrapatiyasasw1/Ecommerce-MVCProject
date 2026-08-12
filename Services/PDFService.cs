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
        public byte[] GeneratepdfInvoice(Order order)
        {
            var pdf = Document.Create(x =>
            {
                x.Page(page =>
                {
                    page.Content().Column(c =>
                    {
                        c.Item().Text($"Order Number: {order.OrderNumber}");
                        c.Item().Text($"Order Date: {order.OrderDate:dd-MM-yyyy}");
                        c.Item().Text($"Customer Email: {order.Customer.User.Email}");

                        c.Item().PaddingVertical(10);

                        c.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Product
                                columns.RelativeColumn(2); // Unit Price
                                columns.RelativeColumn(1); // Quantity
                                columns.RelativeColumn(2); // Total
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Product");
                                header.Cell().Text("Unit Price");
                                header.Cell().Text("Quantity");
                                header.Cell().Text("Total");
                            });

                            foreach (var item in order.OrderItems)
                            {
                                table.Cell().Text(item.Product.Name);
                                table.Cell().Text(item.UnitPrice.ToString("C"));
                                table.Cell().Text(item.Quantity.ToString());
                                table.Cell().Text(
                                    (item.UnitPrice * item.Quantity).ToString("C")
                                );
                            }

                            table.Cell().ColumnSpan(3).Text("Grand Total");
                            table.Cell().Text(
                                order.OrderItems
                                    .Sum(x => x.UnitPrice * x.Quantity)
                                    .ToString("C")
                            );
                        });
                    });
                });
            });

            return pdf.GeneratePdf();
        }


    }
}
