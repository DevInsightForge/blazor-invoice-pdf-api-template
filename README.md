# Invoice PDF Generator Template

A minimal `.NET 10` template that demonstrates dynamic PDF generation from strongly-typed Blazor invoice components rendered to HTML and converted to PDF with `PuppeteerPagePool`.

## Core Idea

This project shows how to build a document-generation API where:

1. Data is generated (or can be supplied from your own domain/service layer).
2. A strongly-typed Blazor component renders the invoice HTML server-side.
3. A pooled Chromium page converts that HTML into a print-ready PDF.
4. A single API endpoint returns the PDF file.

It is designed as a **template foundation** for reusable, maintainable document rendering pipelines.

## What This Demonstrates

- Dynamic invoice generation using typed models.
- Component-based document templates (header, info panel, item table, totals, message).
- Print/PDF-friendly monochrome layout with responsive behavior.
- Efficient PDF rendering via reusable browser/page pooling.
- Minimal API delivery (`GET /api/invoices/pdf`) for immediate integration.

## Setup

## Prerequisites

- .NET SDK `10.x`
- Windows/Linux/macOS environment capable of running Chromium (Puppeteer)

## Install and Run

```bash
git clone <your-repo-url>
cd Invoice-Pdf-Generator
dotnet restore
dotnet run --project InvoicePdf.WebAPI
```

Default endpoint:

- `GET /api/invoices/pdf` -> returns `application/pdf`

Development API docs (Swagger UI):

- `/swagger` (enabled in Development environment)

## Runtime Options

Page format is selected per request via query param:

- `pageFormat=A4` (default)
- `pageFormat=Letter`

Examples:

- `GET /api/invoices/pdf`
- `GET /api/invoices/pdf?pageFormat=A4`
- `GET /api/invoices/pdf?pageFormat=Letter`

## How It Operates

Runtime pipeline:

1. Endpoint receives request (`GET /api/invoices/pdf`).
2. Invoice data factory creates a dynamic invoice model.
3. Blazor `HtmlRenderer` renders invoice components into HTML.
4. `PuppeteerPagePool` loads HTML and prints PDF bytes.
5. API responds with file download (`application/pdf`).

This separation keeps document styling and layout in template components while preserving backend service boundaries.

## Extension Possibilities

Use this template to build:

- Billing invoices for SaaS, agencies, professional services
- Quotations and estimates
- Purchase orders
- Credit notes
- Delivery notes
- Subscription renewal notices
- Payroll slips and expense statements
- Branded customer statements per tenant/client

You can add more template components (e.g., receipt, quote, statement) and route each to its own endpoint or parameterized selector.

## Production Use Cases

- Generate customer-ready PDFs on demand from business events.
- Batch export invoices for accounting periods.
- Produce tenant-specific branded documents from one backend.
- Serve PDFs directly to frontend apps, portals, or external integrations.
- Keep templates versioned in source control with deterministic rendering rules.

## Notes

- Logo rendering supports optional image URL and embedded SVG fallback component.
- Data generation currently uses `Bogus` for template/demo behavior.
- Replace the random data factory with your real application data source when integrating into production systems.
