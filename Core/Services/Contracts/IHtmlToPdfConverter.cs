namespace Core.Services.Contracts;

public interface IHtmlToPdfConverter
{
	Task<byte[]> ConvertFromHTMLFile(string filePath);

	Task<(byte[] PdfBytes, TimeSpan Duration, long MemoryUsed)> ConvertWithPerfTracking(string filePath);
}
