using Core.Services.Contracts;
using PDForgePlayWrite.Service;

public class HtmlToPdfConverterFactory : IHtmlToPdfConverterFactory
{
	private readonly IServiceProvider _serviceProvider;

	public HtmlToPdfConverterFactory(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public IHtmlToPdfConverter Get(string engineName)
	{
		return engineName.ToLower() switch
		{
			"ironpdf" => _serviceProvider.GetRequiredService<IronPDFConverterService>(),
			_ => throw new ArgumentException($"Unknown engine {engineName}")
		};
	}
}

