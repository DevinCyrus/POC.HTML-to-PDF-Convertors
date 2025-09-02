using Core.Services.Contracts;
using Microsoft.Playwright;
using System.Diagnostics;

namespace MicrosoftPlaywright.Service;

public class MicrosoftPlaywrightConverterService : IHtmlToPdfConverter
{
	private readonly IPlaywright _playwright;
	private readonly IBrowser _browser;

	public MicrosoftPlaywrightConverterService(IPlaywright playwright, IBrowser browser)
	{
		/// Requires one time installation/download of browsers - which may need further considerations for our prod env.
		/// Should just need a once off run - perhaps per deployment/release

		/// Was suggested to run below script to install browsers, but I had issues
		/// pwsh .\playwright.ps1 install

		/// Alternative for running the installation - but would require some logic/check to ensure we only run this when needed
		/// Microsoft.Playwright.Program.Main(["install"]);

		_playwright = playwright;
		_browser = browser;
	}

	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		//using var playwright = await Playwright.CreateAsync();
		//await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		//{
		//	Headless = true
		//});

		// Create an isolated browser context for each request (safe for parallel use)
		var browserContext = await _browser.NewContextAsync();

		var page = await browserContext.NewPageAsync();

		var fileUri = new Uri(Path.GetFullPath(filePath)).AbsoluteUri;
		await page.GotoAsync(fileUri, new PageGotoOptions
		{
			WaitUntil = WaitUntilState.NetworkIdle
		});

		// Use screen media to preserve browser layout
		await page.EmulateMediaAsync(new() { Media = Media.Screen });

		var pdfBytes = await page.PdfAsync(new PagePdfOptions
		{
			Format = "A4"
		});

		await browserContext.CloseAsync();
		return pdfBytes;
	}

	public async ValueTask DisposeAsync()
	{
		await _browser.CloseAsync();
		_playwright.Dispose();
	}

	public async Task<(byte[] PdfBytes, TimeSpan Duration, long MemoryUsed)> ConvertWithPerfTracking(string filePath)
	{
		// Warm up garbage collector so memory baseline is clean
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		long beforeMemory = GC.GetTotalMemory(true);

		var stopwatch = Stopwatch.StartNew();

		using var playwright = await Playwright.CreateAsync();
		await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = true
		});

		var page = await browser.NewPageAsync();

		var fileUri = new Uri(Path.GetFullPath(filePath)).AbsoluteUri;
		await page.GotoAsync(fileUri, new PageGotoOptions
		{
			WaitUntil = WaitUntilState.NetworkIdle
		});

		// Use screen media to preserve browser layout - can have varied results
		//await page.EmulateMediaAsync(new() { Media = Media.Screen });

		var pdfBytes = await page.PdfAsync(new PagePdfOptions
		{
			Format = "A4"
		});

		await browser.CloseAsync();

		stopwatch.Stop();

		long afterMemory = GC.GetTotalMemory(false);
		long memoryUsed = afterMemory - beforeMemory;

		return (pdfBytes, stopwatch.Elapsed, memoryUsed);
	}
}
