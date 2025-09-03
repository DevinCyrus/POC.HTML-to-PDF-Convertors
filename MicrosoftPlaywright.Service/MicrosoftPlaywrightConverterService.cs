using Core.Services.Contracts;
using Microsoft.Playwright;

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

	public async ValueTask DisposeAsync()
	{
		await _browser.CloseAsync();
		_playwright.Dispose();
	}

	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		#region Transient headless browser implementation
		//using var playwright = await Playwright.CreateAsync();
		//await using var browserContext = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		//{
		//	Headless = true
		//});
		#endregion

		// Create an isolated browser context for each request
		var browserContext = await _browser.NewContextAsync();

		// Open new page on the new browser context
		var page = await browserContext.NewPageAsync();

		// Load HTML on the new page
		await page.GotoAsync(filePath);

		// Alternatively, load the page with specific GoToOptions if necessary
		// https://playwright.dev/dotnet/docs/api/class-page#page-goto
		//await page.GotoAsync(fileUri, new PageGotoOptions
		//{
		//	WaitUntil = WaitUntilState.NetworkIdle
		//});

		// Use screen media to preserve browser layout - Have varied results during testing
		// https://playwright.dev/dotnet/docs/api/class-page#page-emulate-media
		// await page.EmulateMediaAsync(new() { Media = Media.Screen });

		// Generate PDF from loaded HTML
		var pdfBytes = await page.PdfAsync(new PagePdfOptions
		{
			Format = "A4"
		});

		// Close the browser context and any pages for this session
		await browserContext.CloseAsync();

		return pdfBytes;
	}
}
