# W01 Assignment Notes

## 1. Web API evidence

The `PizzaApi` project contains the ASP.NET Core controller CRUD API from the module. The starting pizza list includes the original records plus one additional record, `Pepperoni Feast`.

Working GET request:

```http
GET http://127.0.0.1:5240/pizza
Status: 200 OK

[
  { "id": 1, "name": "Classic Italian", "isGlutenFree": false },
  { "id": 2, "name": "Veggie", "isGlutenFree": true },
  { "id": 3, "name": "Pepperoni Feast", "isGlutenFree": false }
]
```

Working POST request:

```http
POST http://127.0.0.1:5240/pizza
Content-Type: application/json

{ "name": "Hawaiian", "isGlutenFree": false }

Status: 201 Created

{ "id": 4, "name": "Hawaiian", "isGlutenFree": false }
```

Working PUT request:

```http
PUT http://127.0.0.1:5240/pizza/4
Content-Type: application/json

{ "id": 4, "name": "Hawaiian Deluxe", "isGlutenFree": true }

Status: 204 No Content
```

Working DELETE request:

```http
DELETE http://127.0.0.1:5240/pizza/4

Status: 204 No Content
```

## 2. Sales summary function

Text copy of the working sales summary function from `SalesReportApp/Program.cs`:

```csharp
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
```
