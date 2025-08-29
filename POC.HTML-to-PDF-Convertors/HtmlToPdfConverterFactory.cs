using Core.Services.Contracts;
using MicrosoftPlaywright.Service;
using PDForgePlayWrite.Service;
using Puppeteer.Service;

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
			"puppeteer" => _serviceProvider.GetRequiredService<PuppeteerConverterService>(),
			"microsoftplaywright" => _serviceProvider.GetRequiredService<MicrosoftPlaywrightConverterService>(),
			_ => throw new ArgumentException($"Unknown engine {engineName}")
		};
	}
}

