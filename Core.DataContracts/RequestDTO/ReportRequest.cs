namespace DataContracts.RequestDTO;

public class ReportRequest
{
	// Specify the SDK to use for PDF generation (puppeteer, ironpdf, playwright)
	public string SDK { get; set; } = "puppeteer";

	public string Title { get; set; } = "FinOps PDF Report";

	// Section toggles
	public bool IncludeLineCharts { get; set; } = true;
	public bool IncludeBarCharts { get; set; } = true;
	public bool IncludeStackedCharts { get; set; } = true;
	public bool IncludeMixedCharts { get; set; } = true;
	public bool IncludePieCharts { get; set; } = true;
	public bool IncludeDonutCharts { get; set; } = true;
	public bool IncludeRadarCharts { get; set; } = true;
	public bool IncludeAreaCharts { get; set; } = true;
}

