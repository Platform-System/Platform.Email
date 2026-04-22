using System.Text;
using Platform.Contracts.Messages.Emails;

namespace Platform.Email.Formatters;

public static class InvoiceEmailFormatter
{
    public static string Format(string userName, OrderInvoiceEmailRequested message)
    {
        var builder = new StringBuilder();

        builder.AppendLine("<div style=\"font-family:Arial,sans-serif;max-width:640px;margin:0 auto;line-height:1.5;\">");
        builder.AppendLine($"<h2>Order #{message.OrderCode}</h2>");
        builder.AppendLine($"<p>Hello {System.Net.WebUtility.HtmlEncode(userName)},</p>");
        builder.AppendLine("<p>Your payment was successful. Here is your invoice summary:</p>");
        builder.AppendLine("<table style=\"width:100%;border-collapse:collapse;margin-top:16px;\">");
        builder.AppendLine("<thead><tr>");
        builder.AppendLine("<th style=\"text-align:left;border-bottom:1px solid #ddd;padding:8px;\">Product</th>");
        builder.AppendLine("<th style=\"text-align:right;border-bottom:1px solid #ddd;padding:8px;\">Quantity</th>");
        builder.AppendLine("<th style=\"text-align:right;border-bottom:1px solid #ddd;padding:8px;\">Price</th>");
        builder.AppendLine("</tr></thead>");
        builder.AppendLine("<tbody>");

        foreach (var item in message.Items)
        {
            builder.AppendLine("<tr>");
            builder.AppendLine($"<td style=\"padding:8px;border-bottom:1px solid #f0f0f0;\">{System.Net.WebUtility.HtmlEncode(item.ProductName)}</td>");
            builder.AppendLine($"<td style=\"padding:8px;text-align:right;border-bottom:1px solid #f0f0f0;\">{item.Quantity}</td>");
            builder.AppendLine($"<td style=\"padding:8px;text-align:right;border-bottom:1px solid #f0f0f0;\">{item.Price:N0}</td>");
            builder.AppendLine("</tr>");
        }

        builder.AppendLine("</tbody></table>");
        builder.AppendLine($"<p style=\"margin-top:16px;\"><strong>Total:</strong> {message.TotalAmount:N0}</p>");
        builder.AppendLine($"<p><strong>Created at:</strong> {message.CreatedAt:yyyy-MM-dd HH:mm:ss}</p>");
        builder.AppendLine("<p>Thank you for shopping with us.</p>");
        builder.AppendLine("</div>");

        return builder.ToString();
    }
}
