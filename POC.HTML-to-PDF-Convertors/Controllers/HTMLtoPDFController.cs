using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace POC.HTML_to_PDF_Convertors.Controllers;

[ApiController]
[Route("[controller]")]
public class HTMLtoPDFController : ControllerBase
{
	private readonly IHtmlToPdfConverterFactory _factory;
	private readonly string _testReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "finops-report.html");

	public HTMLtoPDFController(IHtmlToPdfConverterFactory factory)
	{
		_factory = factory;
	}

	[HttpGet("IronPDFSDK/{outputFileName}")]
	public async Task<IActionResult> GetIronPDFGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("ironpdf");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("PuppeteerSDK/{outputFileName}")]
	public async Task<IActionResult> GetPuppeteerGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("puppeteer");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}

	[HttpGet("PDForgePlaywrightSDK/{outputFileName}")]
	public async Task<IActionResult> GetMicrosoftPlaywrightGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("microsoftplaywright");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}
}
