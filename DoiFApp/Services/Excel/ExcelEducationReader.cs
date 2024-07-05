using DoiFApp.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Slicer.Style;
using System.Windows;

namespace DoiFApp.Services.Excel
{
    public class ExcelEducationReader : IEducationReader
    {
        public Task<List<EducationTeacherModel>> ReadFromFile(string fileName)
        {
            using var package = new ExcelPackage(fileName);
            var data = package.Workbook.Worksheets[1];
            var unqueIds = new List<int>();
            foreach (var cell in data.Cells["A"]) // 
            {
                
            }

            return Task.FromResult(new List<EducationTeacherModel>());
        }
    }
}
