namespace DataContracts.ComponentModels;

public class LineChartModel
{
	public string Title { get; set; } = "Monthly Cloud Spend Trend";
	public ChartDataModel Data { get; set; } = new ChartDataModel();
}
