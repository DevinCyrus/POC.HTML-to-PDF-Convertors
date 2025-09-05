using Core.Services.Contracts;

namespace PDForgePlayWrite.Service;

public class IronPDFConverterService : IHtmlToPdfConverter
{
	private readonly ChromePdfRenderer _renderer;

	public IronPDFConverterService()
	{
		// Initialize ChromePdfRenderer for file conversion
		_renderer = new ChromePdfRenderer();

		//// Can optionally set global defaults here, e.g.
		//_renderer.RenderingOptions.MarginTop = 10;
		//_renderer.RenderingOptions.MarginBottom = 10;
		//_renderer.RenderingOptions.CssMediaType = PdfCssMediaType.Screen;
		//// etc...
	}

	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		// Iron PDF handles the headless browser setup and PDF generation in a single call
		// Page styling/RenderingOptions should be set on the _renderer before calling the below method to generate a pdf
		var pdf = await _renderer.RenderHtmlFileAsPdfAsync(filePath);

		return pdf.BinaryData;
	}
}
