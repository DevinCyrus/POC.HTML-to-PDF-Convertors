namespace Core.Services.Contracts;

public interface IHtmlToPdfConverter
{
	Task<byte[]> ConvertFromHTMLFile(string filePath);
	Task<byte[]> ConvertFromHTMLString(string html);
}
