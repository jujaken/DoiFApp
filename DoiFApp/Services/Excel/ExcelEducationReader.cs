using DoiFApp.Models;
using OfficeOpenXml;
using System.IO;

namespace DoiFApp.Services.Excel
{
    public class ExcelEducationReader : IEducationReader
    {
        public Task<List<EducationTeacherModel>> ReadFromFile(string fileName)
        {
            using var package = new ExcelPackage(fileName);
            var data = package.Workbook.Worksheets["Расчет"];

            var outData = new List<EducationTeacherModel>();

            var numsStr = Enumerable.Range(1, 99).Select(n => n.ToString());

            for (int i = 1; i <= data.Rows.Count(); i++)
            {
                var cell = data.Cells[i, 1];
                var id = numsStr.Intersect([cell.Value?.ToString() ?? ""]);
                if (!id.Any()) continue;
                var teacher = new EducationTeacherModel(data.Cells[i, 2].Value.ToString()!)
                {
                    Works1 = GetWorkData(data, 15, i),
                    Works2 = GetWorkData(data, 80, i)
                };
                outData.Add(teacher);
            }

            return Task.FromResult(outData);
        }

        private const int TittleRow = 8;
        private const string WorkStr = "лекции\tсеминары\tпрактические занятия в группе\tпрактические занятия в подгруппе\tучения, д/и, круглый стол\tконсультации перед экзаменами\tтекущие консультации\tвнеаудиторное чтение\tпрактика руководство\tВКР   руководство\tкурсовая работа\tконтрольная работа аудиторная\tконтрольная работа домашняя\tпроверка практикума, реферата\tпроверка лабораторной работы\tзащита практики\tзачет устный\tзачет письменный\tвступительные испытания\tэкзамены\tгосударственные экзамены\tвступительные и кандитатские экзамены (адъюнктура)\tруководство адъюнктами";
        private static readonly IEnumerable<string> workStrSplit = WorkStr.Split('\t', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

        private static Dictionary<string, double> GetWorkData(ExcelWorksheet data, int startColumn, int valueRow)
        {
            var workData = new Dictionary<string, double>();
            for (int i = startColumn; workData.Count != workStrSplit.Count(); i++)
            {
                var tittleCell = data.Cells[TittleRow, i];
                if (tittleCell == null || tittleCell.Value == null) continue;
                var tittle = tittleCell.Value.ToString();
                if (!workStrSplit.Contains(tittle)) continue;
                var valueCell = data.Cells[valueRow, i];
                var value = valueCell == null || valueCell.Value == null ? 0 : (double)valueCell.Value;
                workData.Add(tittle!, value);
                File.AppendAllText("log.txt", $"R{valueRow}C{i} : {value}\n");
            }
            return workData;
        }
    }
}
