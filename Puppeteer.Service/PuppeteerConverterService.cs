using Core.Services.Contracts;
using PuppeteerSharp;
using System.Diagnostics;

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
		// This browser download will likely need to be handled more elegantly in a production environment
		//await new BrowserFetcher().DownloadAsync();
		//using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
		//using var page = await browser.NewPageAsync();
		#endregion

		using var page = await _browser.NewPageAsync();

		await page.GoToAsync(filePath);
		return await page.PdfDataAsync();
	}

	public async Task<(byte[] PdfBytes, TimeSpan Duration, long MemoryUsed)> ConvertWithPerfTracking(string filePath)
	{
		// Warm up garbage collector so memory baseline is clean
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		long beforeMemory = GC.GetTotalMemory(true);

		var stopwatch = Stopwatch.StartNew();

		#region Transient headless browser implementation
		// This browser download will likely need to be handled more elegantly in a production environment
		//await new BrowserFetcher().DownloadAsync();
		//using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
		//using var page = await browser.NewPageAsync();
		#endregion

		using var page = await _browser.NewPageAsync();

		await page.GoToAsync(filePath);


		stopwatch.Stop();

		long afterMemory = GC.GetTotalMemory(false);
		long memoryUsed = afterMemory - beforeMemory;

		return (await page.PdfDataAsync(), stopwatch.Elapsed, memoryUsed);
	}
}
