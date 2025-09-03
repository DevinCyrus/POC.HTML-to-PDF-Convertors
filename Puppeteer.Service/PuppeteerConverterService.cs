using Core.Services.Contracts;
using PuppeteerSharp;

namespace Puppeteer.Service;

public class PuppeteerConverterService : IHtmlToPdfConverter, IAsyncDisposable
{
	private readonly IBrowser _browser;

	public PuppeteerConverterService(IBrowser browser)
	{
		_browser = browser;
	}

	public async ValueTask DisposeAsync()
	{
		if (_browser != null)
		{
			await _browser.CloseAsync();
		}
	}

	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		#region Transient headless browser implementation
		// The below implementation has major performance drawbacks - currently implemented as singleton at startup to reduce overhead
		// This browser download will likely need to be handled more elegantly in a production environment
		//await new BrowserFetcher().DownloadAsync();
		//using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
		//using var page = await browser.NewPageAsync();
		#endregion

		// Open new page on singleton headless browser
		using var page = await _browser.NewPageAsync();

		// Load HTML on the new page
		await page.GoToAsync(filePath);

		// Generate PDF from loaded HTML
		var pdfBytes = await page.PdfDataAsync();

		// Manually close page (due to singleton context for headless browser)
		await page.CloseAsync();

		return pdfBytes;
	}

}
