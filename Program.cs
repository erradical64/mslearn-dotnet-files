// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
//using System.Environment.SpecialFolder;

//Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "stores","201","newDir"));
//bool doesDirectoryExist = Directory.Exists(filePath);
//File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "greeting.txt"), "Hello World!");
string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);    
var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");


var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);     
var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");
AddFileData(salesTotalDir, salesFiles);

//File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), String.Empty);

Console.WriteLine();


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

void AddFileData (string folderName, IEnumerable<string> salesFiles)
{
    File.AppendAllText(Path.Combine(folderName, "sales_summary.txt"), $"Sales Summary{Environment.NewLine}----------------------------{Environment.NewLine}Total Sales:{salesTotal:n2}{Environment.NewLine}{Environment.NewLine}Details:{Environment.NewLine}");
    
    foreach (var file in salesFiles)
    {      
        // Get filename in "file"
        var founder = file.ToString().IndexOf($"stores{Path.DirectorySeparatorChar}");
        string filenamed = file.ToString().Substring(founder);

        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        File.AppendAllText(Path.Combine(folderName, "sales_summary.txt"),$"    {filenamed} : ${data?.Total:n2} {Environment.NewLine}{Environment.NewLine}");
    }
    
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }
    string returned = salesTotal.ToString("00");
    return salesTotal;
}

record SalesData (double Total);