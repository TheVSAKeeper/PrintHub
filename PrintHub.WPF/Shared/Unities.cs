using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Win32;
using OfficeOpenXml;
using PrintHub.WPF.Endpoints.OrderEndpoints.ViewModels;

namespace PrintHub.WPF.Shared;

public class Unities
{
    public static void SaveOrdersToXml(List<OrderViewModel> orders, string filePath)
    {
        XmlSerializer serializer = new(typeof(List<OrderViewModel>));

        using TextWriter writer = new StreamWriter(filePath);

        serializer.Serialize(writer, orders);
    }

    public static void SaveOrdersToExcel(List<OrderViewModel> orders, string filePath)
    {
        using ExcelPackage package = new();

        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Orders");

        int column = 1;

        foreach (PropertyInfo propertyInfo in typeof(OrderViewModel).GetProperties())
        {
            worksheet.Cells[1, column].Value = propertyInfo.Name;
            column++;
        }

        int row = 2;

        foreach (OrderViewModel order in orders)
        {
            column = 1;

            foreach (PropertyInfo propertyInfo in typeof(OrderViewModel).GetProperties())
            {
                worksheet.Cells[row, column].Value = propertyInfo.GetValue(order);
                column++;
            }

            row++;
        }

        package.SaveAs(new FileInfo(filePath));
    }

    public static void SaveToExcel(List<OrderViewModel> list)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "Excel Files|*.xlsx",
            FileName = $"OrdersData_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == false)
            return;

        string filePath = saveFileDialog.FileName;

        SaveOrdersToExcel(list, filePath);
    }

    public static void SaveToXml(List<OrderViewModel> list)
    {
        SaveFileDialog saveFileDialog = new()
        {
            Filter = "XML Files|*.xml",
            FileName = $"OrdersData_{DateTime.Now:yyyyMMdd_HHmmss}.xml"
        };

        if (saveFileDialog.ShowDialog() == false)
            return;

        string filePath = saveFileDialog.FileName;

        SaveOrdersToXml(list, filePath);
    }
}