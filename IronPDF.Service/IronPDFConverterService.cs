using Core.Services.Contracts;
using System.Diagnostics;

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

	public async Task<(byte[] PdfBytes, TimeSpan Duration, long MemoryUsed)> ConvertWithPerfTracking(string filePath)
	{
		return await Task.Run(() =>
		{
			// Warm up garbage collector so memory baseline is clean
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			long beforeMemory = GC.GetTotalMemory(true);

			var stopwatch = Stopwatch.StartNew();

			var pdf = _renderer.RenderHtmlFileAsPdf(filePath);

			stopwatch.Stop();

			long afterMemory = GC.GetTotalMemory(false);
			long memoryUsed = afterMemory - beforeMemory;

			return (pdf.BinaryData, stopwatch.Elapsed, memoryUsed);
		});
	}
}
