using Core.Services.Contracts;
using Microsoft.Playwright;
using MicrosoftPlaywright.Service;
using PDForgePlayWrite.Service;
using Puppeteer.Service;
using PuppeteerSharp;
using SelectPDF.Service;

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
builder.Services.AddScoped<SelectPDFConverterService>();
builder.Services.AddScoped<IHtmlToPdfConverterFactory, HtmlToPdfConverterFactory>();

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
