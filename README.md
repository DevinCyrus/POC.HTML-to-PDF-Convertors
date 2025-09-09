# HTML to PDF Converter POC

This repository is a **proof-of-concept (POC)** comparing different .NET SDKs for generating PDFs from HTML content.  
The project evaluates **PuppeteerSharp**, **PlaywrightSharp**, and **IronPDF**, with additional experiments using **Razor templating** for dynamic HTML generation.

---

### Overview

The goal of this POC is to:

- Compare performance, output size, and ease of integration between the three SDKs.  
- Evaluate how Razor templating can be used to dynamically generate report HTML before conversion.  
- Establish a baseline service-layer design for HTML-to-PDF conversion in a .NET API.  

---

### SDKs Tested

### PuppeteerSharp
- Wraps Chromium browser automation.  
- Handles Chromium installation automatically.  
- Good balance between control and ease-of-use.  

### PlaywrightSharp
- Similar to PuppeteerSharp but requires developers to manage browser binaries.  
- Provides the most fine-grained control over browser options.  

### IronPDF
- Commercial SDK (requires license for production use).  
- Embeds its own Chromium renderer (no browser management needed).  
- Simplest integration but less flexible.  

---

### Performance Logging

PDF generation metrics (duration + output size) are written to PdfPerfLogs/perf-results.csv.
This folder is ignored by Git via .gitignore.

---

### License

MIT (for this POC code).
Note: IronPDF is commercial software and requires a valid license for production use.

