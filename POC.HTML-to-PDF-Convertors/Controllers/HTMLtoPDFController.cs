using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace POC.HTML_to_PDF_Convertors.Controllers;

[ApiController]
[Route("[controller]")]
public class HTMLtoPDFController : ControllerBase
{
	private readonly IHtmlToPdfConverterFactory _factory;
	private readonly string _genericReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "finops-report-apex-charts (disabled animations).html");
	private readonly string _cloudOverviewReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview.html");
	private readonly string _cleanedCloudOverviewReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview - Cleaned.html");
	private readonly string _serviceReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Report Service.html");
	private readonly string _vendorReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Updated Report Vendor.html");

	private string _testReportPath;

	public HTMLtoPDFController(IHtmlToPdfConverterFactory factory)
	{
		_factory = factory;

		_testReportPath = _cleanedCloudOverviewReportPath;
	}

	[HttpGet("IronPDFSDK/{outputFileName}")]
	public async Task<IActionResult> GetIronPDFGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("ironpdf");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("IronPDFSDK/{outputFileName}/TrackPerformance")]
	public async Task<IActionResult> GetIronPDFGeneratedPDFWithPerformance([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("ironpdf");
		var (pdfBytes, duration, memoryUsed) = await converter.ConvertWithPerfTracking(_testReportPath);

		// Add perf info to headers so you can track in Postman / browser
		Response.Headers["X-PDF-Generation-Time-ms"] = duration.TotalMilliseconds.ToString("F0");
		Response.Headers["X-PDF-Memory-Used-bytes"] = memoryUsed.ToString();

		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("PuppeteerSDK/{outputFileName}")]
	public async Task<IActionResult> GetPuppeteerGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("puppeteer");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("PuppeteerSDK/{outputFileName}/TrackPerformance")]
	public async Task<IActionResult> GetPuppeteerGeneratedPDFWithPerformance([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("puppeteer");
		var (pdfBytes, duration, memoryUsed) = await converter.ConvertWithPerfTracking(_testReportPath);

		// Add perf info to headers so you can track in Postman / browser
		Response.Headers["X-PDF-Generation-Time-ms"] = duration.TotalMilliseconds.ToString("F0");
		Response.Headers["X-PDF-Memory-Used-bytes"] = memoryUsed.ToString();

		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("PlaywrightSDK/{outputFileName}")]
	public async Task<IActionResult> GetMicrosoftPlaywrightGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("microsoftplaywright");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("PlaywrightSDK/{outputFileName}/TrackPerformance")]
	public async Task<IActionResult> GetMicrosoftPlaywrightGeneratedPDFWithPerformance([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("microsoftplaywright");
		var (pdfBytes, duration, memoryUsed) = await converter.ConvertWithPerfTracking(_testReportPath);

		// Add perf info to headers so you can track in Postman / browser
		Response.Headers["X-PDF-Generation-Time-ms"] = duration.TotalMilliseconds.ToString("F0");
		Response.Headers["X-PDF-Memory-Used-bytes"] = memoryUsed.ToString();

		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	// Hiding endpoint SelectPDF cannot handle dynamic javascript

	//[HttpGet("SelectPDFSDK/{outputFileName}")]
	//public async Task<IActionResult> GetSelectPDFGeneratedPDF([FromRoute] string outputFileName)
	//{
	//	var converter = _factory.Get("selectpdf");
	//	var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
	//	return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	//}
}
