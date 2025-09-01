using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace POC.HTML_to_PDF_Convertors.Controllers;

[ApiController]
[Route("[controller]")]
public class HTMLtoPDFController : ControllerBase
{
	private readonly IHtmlToPdfConverterFactory _factory;
	private readonly string _testReportPath = Path.Combine(AppContext.BaseDirectory, "MockData", "finops-report-apex-charts (disabled animations).html");

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

	[HttpGet("PlaywrightSDK/{outputFileName}")]
	public async Task<IActionResult> GetMicrosoftPlaywrightGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("microsoftplaywright");
		var pdfBytes = await converter.ConvertFromHTMLFile(_testReportPath);
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
