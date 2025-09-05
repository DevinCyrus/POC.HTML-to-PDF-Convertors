namespace POC.HTML_to_PDF_Convertors;

public class PerformanceLogger
{
	private readonly string _logPath;

	public PerformanceLogger(string logDir = "PdfPerfLogs")
	{
		Directory.CreateDirectory(logDir);
		_logPath = Path.Combine(logDir, "perf-results.csv");

		if (!File.Exists(_logPath))
		{
			File.WriteAllText(_logPath, "Timestamp,SDK,ReportName,Duration(ms),Size(KB)\n");
		}
	}

	public void Log(string sdkName, string reportName, string durationMs, string sizeKB)
	{
		var line = $"{DateTime.UtcNow:O},{sdkName},{reportName},{durationMs},{sizeKB}\n";
		File.AppendAllText(_logPath, line);
	}
}

