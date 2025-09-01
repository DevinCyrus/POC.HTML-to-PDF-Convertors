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

	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		return await Task.Run(() =>
		{
			var pdf = _renderer.RenderHtmlFileAsPdf(filePath);
			return pdf.BinaryData;
		});
	}
}
