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

            var (teacherRows, endId) = GetTeacherRows(data);
            teacherRows.Add(endId);

            for (int i = 0; i < teacherRows.Count - 1; i++) // row
            {
                var teacherId = teacherRows[i];
                var teacherCell = data.Cells[teacherId, 2];
                var teacher = new EducationTeacherModel(teacherCell.Value.ToString()!);

                for (int j = teacherId + 1; j < teacherRows[i + 1]; j++) // row
                {
                    var work1 = GetWorkTeacher(data, 15, 2, j);
                    if (work1 != null)
                        teacher.Works1.Add(work1);

                    var work2 = GetWorkTeacher(data, 80, 67, j);
                    if (work2 != null)
                        teacher.Works2.Add(work2);
                }

                outData.Add(teacher);
            }

            return Task.FromResult(outData);
        }

        private static (List<int>, int) GetTeacherRows(ExcelWorksheet data)
        {
            var teacherRows = new List<int>();
            var endId = 0;

            var numsStr = Enumerable.Range(1, 99).Select(n => n.ToString());

            for (int i = 1; i <= data.Rows.Count(); i++)
            {
                var cell = data.Cells[i, 1];
                var value = cell.Value?.ToString();
                var id = numsStr.Intersect([value ?? ""]);
                if (id.Any())
                    teacherRows.Add(i);
                if (value != null && value.Contains("итого:", StringComparison.CurrentCultureIgnoreCase))
                    endId = i;
            }
            return (teacherRows, endId);
        }

        private static EducationWorkModel? GetWorkTeacher(ExcelWorksheet data, int startColumn, int workColumn, int valueRow)
        {
            var workCell = data.Cells[valueRow, workColumn];
            if (workCell == null) return null;

            var workName = data.Cells[valueRow, workColumn].Value?.ToString();
            if (workName == null) return null;

            return new EducationWorkModel(workName)
            {
                TypesAndHours = GetWorkData(data, startColumn, valueRow)
            };
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

                //File.AppendAllText("log.txt", $"R{valueRow}C{i} : {value}\n");
            }
            return workData;
        }
    }
}
