using System.Globalization;
using System.Text;
using System.Text.Json;

string storesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "stores");
Directory.CreateDirectory(Path.Combine(storesDirectory, "201"));
Directory.CreateDirectory(Path.Combine(storesDirectory, "202"));

CreateSampleSalesFile(Path.Combine(storesDirectory, "201", "sales.json"), 22385.32m);
CreateSampleSalesFile(Path.Combine(storesDirectory, "202", "sales.json"), 14391.79m);
CreateSampleSalesFile(Path.Combine(storesDirectory, "sales.json"), 8912.43m);

string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "salesSummary.txt");
GenerateSalesSummaryReport(storesDirectory, reportPath);

Console.WriteLine(File.ReadAllText(reportPath));

static void CreateSampleSalesFile(string filePath, decimal total)
{
    if (File.Exists(filePath))
    {
        return;
    }

    var salesTotal = new SalesTotal(total);
    string json = JsonSerializer.Serialize(salesTotal, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
}

static decimal CalculateSalesTotal(string filePath)
{
    string salesJson = File.ReadAllText(filePath);
    SalesTotal? salesData = JsonSerializer.Deserialize<SalesTotal>(salesJson);

    return salesData?.Total ?? 0;
}

static void GenerateSalesSummaryReport(string storesDirectory, string reportPath)
{
    string[] salesFiles = Directory.GetFiles(storesDirectory, "sales.json", SearchOption.AllDirectories);
    var report = new StringBuilder();
    var salesByFile = new List<(string FileName, decimal Total)>();
    CultureInfo currencyCulture = CultureInfo.GetCultureInfo("en-US");

    foreach (string file in salesFiles)
    {
        decimal fileTotal = CalculateSalesTotal(file);
        salesByFile.Add((Path.GetRelativePath(storesDirectory, file), fileTotal));
    }

    decimal totalSales = salesByFile.Sum(item => item.Total);

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine(currencyCulture, $" Total Sales: {totalSales:C}");
    report.AppendLine();
    report.AppendLine(" Details:");

    foreach ((string fileName, decimal total) in salesByFile.OrderBy(item => item.FileName))
    {
        report.AppendLine(currencyCulture, $"  {fileName}: {total:C}");
    }

    File.WriteAllText(reportPath, report.ToString());
}

public record SalesTotal(decimal Total);
