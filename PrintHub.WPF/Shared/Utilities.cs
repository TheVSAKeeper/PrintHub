using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Win32;
using OfficeOpenXml;

namespace PrintHub.WPF.Shared;

public static class Utilities
{
    private static void SaveOrdersToXml<T>(List<T> orders, string filePath)
    {
        XmlSerializer serializer = new(typeof(List<T>));

        using TextWriter writer = new StreamWriter(filePath);

        serializer.Serialize(writer, orders);
    }

    private static void SaveOrdersToExcel<T>(List<T> orders, string filePath)
    {
        using ExcelPackage package = new();

        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(typeof(T).Name);

        int column = 1;

        foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
        {
            worksheet.Cells[1, column].Value = propertyInfo.Name;
            column++;
        }

        int row = 2;

        foreach (T order in orders)
        {
            column = 1;

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                worksheet.Cells[row, column].Value = propertyInfo.GetValue(order);
                column++;
            }

            row++;
        }

        package.SaveAs(new FileInfo(filePath));
    }

    public static void SaveToExcel<T>(List<T> list)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Excel Files|*.xlsx",
            FileName = $"{typeof(T).Name}Data_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == false)
            return;

        string filePath = saveFileDialog.FileName;

        SaveOrdersToExcel(list, filePath);
    }

    public static void SaveToXml<T>(List<T> list)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "XML Files|*.xml",
            FileName = $"{typeof(T).Name}Data_{DateTime.Now:yyyyMMdd_HHmmss}.xml"
        };

        if (saveFileDialog.ShowDialog() == false)
            return;

        string filePath = saveFileDialog.FileName;

        SaveOrdersToXml(list, filePath);
    }
}