using RazorLight;

namespace ReportTemplating.Service;

public class ReportTemplatingService
{
	private readonly RazorLightEngine _engine;

	public ReportTemplatingService()
	{
		_engine = new RazorLightEngineBuilder()
			.UseFileSystemProject(Path.Combine(AppContext.BaseDirectory, "Report Templates"))
			.UseMemoryCachingProvider()
			.Build();
	}

	public async Task<string> RenderAsync<T>(string templateName, T model)
	{
		return await _engine.CompileRenderAsync(templateName, model);
	}
}
