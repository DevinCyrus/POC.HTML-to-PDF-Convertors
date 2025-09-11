namespace DataContracts.ComponentModels;

public class ChartDataModel
{
	public List<string> Labels { get; set; } = new List<string>();
	public List<decimal> Values { get; set; } = new List<decimal>();
}
