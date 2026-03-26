namespace InvoicePdf.Templates.Rendering;

public static class InvoiceHtmlDocumentComposer
{
    public static string Compose(string body, string invoiceNumber)
    {
        var title = string.IsNullOrWhiteSpace(invoiceNumber) ? "Invoice" : $"Invoice {invoiceNumber}";

        return $$"""
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>{{title}}</title>
  <style>
    * { box-sizing: border-box; }
    body { margin: 0; font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif; color: #1f2937; background: #f3f4f6; }
    .page-wrap { padding: 24px; }
    .invoice { width: 210mm; min-height: 297mm; margin: 0 auto; background: #ffffff; padding: 24mm 18mm; }
    h1,h2,h3,p { margin: 0; }
    table { width: 100%; border-collapse: collapse; }
    th, td { padding: 10px 8px; border-bottom: 1px solid #e5e7eb; }
    th { text-align: left; font-size: 12px; letter-spacing: 0.03em; color: #374151; background: #f9fafb; }
    td { font-size: 13px; }
    .text-right { text-align: right; }
    .meta-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 16px; margin-top: 20px; }
    .party-grid { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 24px; margin-top: 28px; }
    .totals { margin-top: 20px; margin-left: auto; width: 45%; }
    .totals td { border-bottom: 0; }
    .totals tr:last-child td { font-weight: 700; font-size: 16px; border-top: 2px solid #d1d5db; padding-top: 12px; }
    .muted { color: #6b7280; }
    .section-title { font-size: 12px; text-transform: uppercase; letter-spacing: 0.08em; color: #6b7280; margin-bottom: 8px; }
    .header { display: flex; justify-content: space-between; align-items: flex-start; border-bottom: 2px solid #111827; padding-bottom: 16px; }
    .footer { margin-top: 24px; border-top: 1px solid #e5e7eb; padding-top: 12px; font-size: 12px; color: #6b7280; }
    @page { size: A4; margin: 12mm; }
    @media print {
      body { background: #ffffff; }
      .page-wrap { padding: 0; }
      .invoice { width: 100%; min-height: auto; margin: 0; padding: 0; }
    }
  </style>
</head>
<body>
  <div class="page-wrap">
    {{body}}
  </div>
</body>
</html>
""";
    }
}
