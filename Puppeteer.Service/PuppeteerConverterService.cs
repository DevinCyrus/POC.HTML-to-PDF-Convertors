using Core.Services.Contracts;
using PuppeteerSharp;

namespace Puppeteer.Service;

public class PuppeteerConverterService : IHtmlToPdfConverter
{
	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		// This browser download will likely need to be handled more elegantly in a production environment
		await new BrowserFetcher().DownloadAsync();
		using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
		using var page = await browser.NewPageAsync();

		await page.GoToAsync(filePath);
		return await page.PdfDataAsync();
	}
}
