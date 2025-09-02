using Core.Services.Contracts;
using Microsoft.Playwright;
using MicrosoftPlaywright.Service;
using PDForgePlayWrite.Service;
using Puppeteer.Service;
using SelectPDF.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IronPDFConverterService>();
builder.Services.AddScoped<PuppeteerConverterService>();
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



var app = builder.Build();

// Force creation of singleton at startup (not strictly required, but ensures the browser is launched early)
//app.Services.GetRequiredService<IHtmlToPdfConverter>();

//app.MapGet("/", () => "Playwright PDF service is running!");

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
