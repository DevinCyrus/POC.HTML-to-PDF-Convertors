using Core.Services.Contracts;
using MicrosoftPlaywright.Service;
using PDForgePlayWrite.Service;
using Puppeteer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IronPDFConverterService>();
builder.Services.AddScoped<PuppeteerConverterService>();
builder.Services.AddScoped<MicrosoftPlaywrightConverterService>();
builder.Services.AddScoped<IHtmlToPdfConverterFactory, HtmlToPdfConverterFactory>();

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
