using Core.Services.Contracts;
using Microsoft.Playwright;

namespace MicrosoftPlaywright.Service;

public class MicrosoftPlaywrightConverterService : IHtmlToPdfConverter
{
	public MicrosoftPlaywrightConverterService()
	{
		/// Requires one time installation/download of browsers - which may need further considerations for our prod env.
		/// Should just need a once off run - perhaps per deployment/release

		/// Was suggested to run below script to install browsers, but I had issues
		/// pwsh .\playwright.ps1 install

		/// Alternative for running the installation - but would require some logic/check to ensure we only run this when needed
		/// Microsoft.Playwright.Program.Main(["install"]);
	}

	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
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

		// Use screen media to preserve browser layout
		await page.EmulateMediaAsync(new() { Media = Media.Screen });

		var pdfBytes = await page.PdfAsync(new PagePdfOptions
		{
			Format = "A4"
		});

		await browser.CloseAsync();
		return pdfBytes;
	}
}
