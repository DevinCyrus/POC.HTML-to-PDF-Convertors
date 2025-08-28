namespace Core.Services.Contracts;

public interface IHtmlToPdfConverterFactory
{
	IHtmlToPdfConverter Get(string engineName);
}
