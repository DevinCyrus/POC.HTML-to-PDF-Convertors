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

	// Dynamic chart data
	public List<string> LineLabels { get; set; } = new();
	public List<decimal> LineValues { get; set; } = new();

	public List<string> BarLabels { get; set; } = new();
	public List<decimal> AwsBarValues { get; set; } = new();
	public List<decimal> AzureBarValues { get; set; } = new();

	public List<string> MixedLabels { get; set; } = new();
	public List<decimal> ActualSpend { get; set; } = new();
	public List<decimal> Budget { get; set; } = new();

	public List<string> CloudVendors { get; set; } = new(); // ["AWS","Azure","GCP","Other"]
	public List<decimal> CloudSpend { get; set; } = new();  // [12800,15350,31200,28700]

	public List<string> RadarLabels { get; set; } = new();
	public List<decimal> Efficiency { get; set; } = new();

	public List<string> AreaLabels { get; set; } = new();
	public List<decimal> AreaValues { get; set; } = new();
}