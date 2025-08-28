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

	[HttpGet("PDForgePlayWriteSDK/{outputFileName}")]
	public IActionResult GetIronPDFGeneratedPDF([FromRoute] string outputFileName)
	{
		var converter = _factory.Get("ironpdf");
		var pdfBytes = converter.ConvertFromHTMLFile(_testReportPath);
		return File(pdfBytes, "application/pdf", outputFileName + ".pdf");
	}
}
