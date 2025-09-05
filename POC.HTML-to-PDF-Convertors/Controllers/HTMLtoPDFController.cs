using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace POC.HTML_to_PDF_Convertors.Controllers;

[ApiController]
[Route("[controller]")]
public class HTMLtoPDFController : ControllerBase
{
	#region Various Mock HTML Reports for testing PDF generation
	private readonly IHtmlToPdfConverterFactory _factory;
	private readonly string _genericReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "finops-report-apex-charts (disabled animations).html");
	private readonly string _chartComparisonReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Chart Testing.html");
	private readonly string _cloudOverviewReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview.html");
	private readonly string _serviceReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Report Service.html");
	private readonly string _vendorReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Updated Report Vendor.html");

	// Report that have been somewhat cleaned up and styled for PDF generation
	private readonly string _cleanedCloudOverviewReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview - Cleaned.html");

	// Duplicates of the above report but with sections moved around
	// These will be used in performance testing so that back to back calls to API are not for the exact same report to avoid potential caching skewing results
	private readonly string _cleanedCloudOverviewReportPathV1 = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview - Cleaned - v1.html");
	private readonly string _cleanedCloudOverviewReportPathV2 = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview - Cleaned - v2.html");
	private readonly string _cleanedCloudOverviewReportPathV3 = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview - Cleaned - v3.html");
	private readonly string _cleanedCloudOverviewReportPathV4 = Path.Combine(AppContext.BaseDirectory, "MockData", "Cloud Overview - Cleaned - v4.html");
	#endregion

	#region Helper class used to switch reports for performance testing
	private void setReportPath(int reportNumber)
	{
		_testReportPath = reportNumber switch
		{
			1 => _cleanedCloudOverviewReportPathV1,
			2 => _cleanedCloudOverviewReportPathV2,
			3 => _cleanedCloudOverviewReportPathV3,
			4 => _cleanedCloudOverviewReportPathV4,
			5 => _cleanedCloudOverviewReportPath,
			6 => _chartComparisonReportPath,
			_ => _genericReportPath
		};
	}
	#endregion

	private string _testReportPath;
	private PerformanceLogger _perfLogger;

	public HTMLtoPDFController(IHtmlToPdfConverterFactory factory, PerformanceLogger perfLogger)
	{
		_factory = factory;
		_perfLogger = perfLogger;

		_testReportPath = _cleanedCloudOverviewReportPath;
	}

	/// <summary>
	/// Generate report PDF using IronPDF and include duration with processing duration in header
	/// </summary>
	/// <param name="reportNumber">There are multiple versions of the same report (1-6) just with varied layout ordering, this is used for performance testing so that calls can be for different reports to try avoid any hidden caching that may skew results.</param>
	/// <param name="outputFileName">The name that the generated report file will have.</param>
	[HttpGet("IronPDFSDK/{reportNumber}/{outputFileName}")]
	public async Task<IActionResult> GetIronPDFGeneratedPDF([FromRoute] string outputFileName, [FromRoute] int reportNumber = 6)
	{
		setReportPath(reportNumber);

		var stopwatch = Stopwatch.StartNew();

		var converter = _factory.Get("ironpdf");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);

		stopwatch.Stop();

		// Log performance stats
		var duration = stopwatch.Elapsed.TotalMilliseconds.ToString("F0");
		var fileSize = ((decimal)pdfBytes.Length / 1024).ToString("F3");
		var reportName = Path.GetFileName(_testReportPath);

		_perfLogger.Log("IronPDF", reportName, duration, fileSize);

		// Attach duration and file size to response headers
		Response.Headers["X-IronPDF-PDF-Generation-Time-ms"] = duration;
		Response.Headers["X-IronPDF-PDF-Generation-Size-KB"] = fileSize;

		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	/// <summary>
	/// Generate report PDF using PuppeteerSharp and include duration with processing duration in header
	/// </summary>
	/// <param name="reportNumber">There are multiple versions of the same report (1-6) just with varied layout ordering, this is used for performance testing so that calls can be for different reports to try avoid any hidden caching that may skew results.</param>
	/// <param name="outputFileName">The name that the generated report file will have.</param>
	[HttpGet("PuppeteerSharpSDK/{reportNumber}/{outputFileName}")]
	public async Task<IActionResult> GetPuppeteerSharpGeneratedPDF([FromRoute] string outputFileName, [FromRoute] int reportNumber = 6)
	{
		setReportPath(reportNumber);

		var stopwatch = Stopwatch.StartNew();

		var converter = _factory.Get("puppeteer");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);

		stopwatch.Stop();

		// Log performance stats
		var duration = stopwatch.Elapsed.TotalMilliseconds.ToString("F0");
		var fileSize = ((decimal)pdfBytes.Length / 1024).ToString("F3");
		var reportName = Path.GetFileName(_testReportPath);

		_perfLogger.Log("PuppeteerSharp", reportName, duration, fileSize);

		// Attach duration and file size to response headers
		Response.Headers["X-Puppeteer-PDF-Generation-Time-ms"] = duration;
		Response.Headers["X-Puppeteer-PDF-Generation-Size-KB"] = fileSize;

		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	/// <summary>
	/// Generate report PDF using Playwright and include duration with processing duration in header
	/// </summary>
	/// <param name="reportNumber">There are multiple versions of the same report (1-6) just with varied layout ordering, this is used for performance testing so that calls can be for different reports to try avoid any hidden caching that may skew results.</param>
	/// <param name="outputFileName">The name that the generated report file will have.</param>
	[HttpGet("PlaywrightSDK/{reportNumber}/{outputFileName}")]
	public async Task<IActionResult> GetMicrosoftPlaywrightGeneratedPDF([FromRoute] string outputFileName, [FromRoute] int reportNumber = 6)
	{
		setReportPath(reportNumber);

		var stopwatch = Stopwatch.StartNew();

		var converter = _factory.Get("playwright");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);

		stopwatch.Stop();

		// Log performance stats
		var duration = stopwatch.Elapsed.TotalMilliseconds.ToString("F0");
		var fileSize = ((decimal)pdfBytes.Length / 1024).ToString("F3");
		var reportName = Path.GetFileName(_testReportPath);

		_perfLogger.Log("Playwright", reportName, duration, fileSize);

		// Attach duration and file size to response headers
		Response.Headers["X-Playwright-PDF-Generation-Time-ms"] = duration;
		Response.Headers["X-Playwright-PDF-Generation-Size-KB"] = fileSize;


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
