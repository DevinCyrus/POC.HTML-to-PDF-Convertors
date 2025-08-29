using Core.Services.Contracts;
using SelectPdf;

namespace SelectPDF.Service;

public class SelectPDFConverterService : IHtmlToPdfConverter
{
	public async Task<byte[]> ConvertFromHTMLFile(string filePath)
	{
		return await Task.Run(() =>
		{
			var converter = new HtmlToPdf();

			PdfDocument doc = converter.ConvertUrl($"file:///{filePath}");

			try
			{
				using (var ms = new MemoryStream())
				{
					doc.Save(ms);
					return ms.ToArray();
				}
			}
			finally
			{
				doc.Close();
			}
		});
	}
}
