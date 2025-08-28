using Core.Services.Contracts;

namespace PDForgePlayWrite.Service;

public class IronPDFConverterService : IHtmlToPdfConverter
{
	private readonly ChromePdfRenderer _renderer;

	public IronPDFConverterService()
	{
		_renderer = new ChromePdfRenderer();

		//// Can optionally set global defaults here, e.g.
		//_renderer.RenderingOptions.MarginTop = 10;
		//_renderer.RenderingOptions.MarginBottom = 10;
		//_renderer.RenderingOptions.CssMediaType = PdfCssMediaType.Screen;
		//// etc...
	}

	public byte[] ConvertFromHTMLFile(string file)
	{
		var pdf = _renderer.RenderHtmlFileAsPdf(file);
		return pdf.BinaryData;
	}
}
