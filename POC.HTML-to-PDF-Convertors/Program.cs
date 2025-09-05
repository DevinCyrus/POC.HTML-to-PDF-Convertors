using Core.Services.Contracts;
using Microsoft.Playwright;
using MicrosoftPlaywright.Service;
using PDForgePlayWrite.Service;
using POC.HTML_to_PDF_Convertors;
using PuppeteerSharp;
using PuppeteerSharp.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IronPDFConverterService>();

// Removed to implement singleton browsers for performance gains
//builder.Services.AddScoped<PuppeteerConverterService>();
//builder.Services.AddScoped<MicrosoftPlaywrightConverterService>();

// Removed as can't handle dynamic javscript
//builder.Services.AddScoped<SelectPDFConverterService>();

builder.Services.AddScoped<IHtmlToPdfConverterFactory, HtmlToPdfConverterFactory>();

builder.Services.AddSingleton<PerformanceLogger>();

// Create Playwright + Browser once for the app lifetime
builder.Services.AddSingleton<MicrosoftPlaywrightConverterService>(sp =>
{
	var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
	var browser = playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
	{
		Headless = true
	}).GetAwaiter().GetResult();

	return new MicrosoftPlaywrightConverterService(playwright, browser);
});

// Create Puppeteer Browser once for the app lifetime
builder.Services.AddSingleton<PuppeteerSharpConverterService>(sp =>
{
	new BrowserFetcher().DownloadAsync().GetAwaiter().GetResult();
	var browser = PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions { Headless = true })
		.GetAwaiter().GetResult();

	return new PuppeteerSharpConverterService(browser);
});

// IronPDF offers a ~7day free trial with no requirements
// If your ~7day trial expires, you can request an extended 30day trial, you will just need to provide an email.
// https://ironpdf.com/#trial-license
// Once you receive your license, add it to your user secrets with the following alias "IronPDF-30day-Trial-License"

// Set the IronPdf license key
IronPdf.License.LicenseKey = builder.Configuration["IronPDF-30day-Trial-License"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
