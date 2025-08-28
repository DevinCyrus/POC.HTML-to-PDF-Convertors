namespace Core.Services.Contracts;

public interface IHtmlToPdfConverter
{
	byte[] ConvertFromHTMLFile(string file);
}
