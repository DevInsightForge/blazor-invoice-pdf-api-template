# Invoice PDF Template Solution Plan (`Invoice-Pdf-Generator`)

## Summary
Create a `.NET 10` multi-project template solution that renders Blazor components to HTML using `HtmlRenderer`, converts the HTML to PDF with `PuppeteerPagePool`, and returns the generated invoice PDF from a Web API endpoint.
Project naming updated to `InvoicePdf.WebAPI` (not `.Api`).
No unit test project will be included.

## Implementation Changes
1. **Solution structure**
1. `InvoicePdf.WebAPI` (ASP.NET Core Web API host + endpoints + DI)
1. `InvoicePdf.Templates` (Razor Class Library with invoice components and HTML renderer extensions)

2. **Root planning artifact**
1. Add root file `PLANS.md` containing this full implementation plan.
1. `PLANS.md` acts as the single execution reference while building the solution.

3. **HTTP API contract**
1. Add `POST /api/invoices/pdf`.
1. Request body: invoice payload (`InvoiceNumber`, dates, seller/buyer info, items, currency, terms).
1. Response: direct PDF bytes with:
`Content-Type: application/pdf`
`Content-Disposition: attachment; filename="invoice-{InvoiceNumber}.pdf"`.

4. **Blazor component HTML rendering**
1. Keep your `HtmlRendererExtensions` pattern (`RenderComponent<T>` + dictionary params + `ParameterView`).
1. Do not add Blazor interactive/server rendering services.
1. Build a renderer service that creates/uses `HtmlRenderer`, renders `InvoiceDocument.razor` with runtime values, and returns full HTML document string (head/body + print CSS).

5. **PDF generation via PuppeteerPagePool**
1. Register `PuppeteerPagePool` in Web API DI with operational defaults (`PoolSize`, `AcquireTimeout`, `ShutdownTimeout`, `ResetTargetUrl`, headless launch args).
1. Use `IPagePool.ExecuteAsync` to:
`SetContentAsync(html)` ? `WaitForNetworkIdleAsync()` ? `PdfDataAsync(...)`.
1. Return produced bytes from the endpoint directly.

6. **Template showcase content**
1. Add example billing invoice component (`InvoiceDocument.razor`) with:
header, seller/buyer blocks, metadata, line-items table, subtotal/tax/total, footer.
1. Add deterministic formatting for dates, money, and totals.
1. Include sample JSON payload for quick manual verification.

## Validation (No Unit Tests)
1. Manual API run and call `POST /api/invoices/pdf` with sample payload.
1. Confirm non-empty PDF response, correct content headers, and valid invoice values rendered in output.
1. Confirm invalid payloads return `400` validation errors.

## Assumptions and Defaults
1. Runtime is `.NET 10`.
1. Output mode is direct file response only.
1. Templates are in separate class library.
1. Initial scope is one invoice template demonstrating dynamic rendering end-to-end.
1. No automated unit tests are part of this implementation.
