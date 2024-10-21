using DoiFApp.Data.Models;
using DoiFApp.Services.Data;
using DoiFApp.Utils;
using OfficeOpenXml;

namespace DoiFApp.Services.Education
{
    public class ExcelEducationReader : IDataReader<EducationData>
    {
        private const int TittleRow = 8;
        private static readonly IEnumerable<string> headers = TableDataUtil.GetHeaders(TableDataUtil.InputCommonTableHeaders);

        public Task<EducationData> Read(string path)
        {
            using var package = new ExcelPackage(path);
            var worksheet = package.Workbook.Worksheets.Where(w => w.Name.Contains("расч", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault()
                ?? throw new Exception("worksheet not found");

            var teachers = new List<EducationTeacherModel>();
            var typeAndHours = new List<EducationTypeAndHourModel>();
            var works = new List<EducationWorkModel>();

            ParseData(worksheet,
                (data, teacher, j) =>
                {
                    var work1 = GetWorkTeacher(data, 15, 2, j);
                    if (work1 != null)
                    {
                        work1.TypesAndHours.ForEach(t => typeAndHours.Add(t));
                        work1.WorkCategory = WorkCategory.PlanFirstSemester;
                        works.Add(work1);
                        work1.Teacher = teacher;
                        teacher.Works.Add(work1);
                    }
                    var work2 = GetWorkTeacher(data, 77, 2, j);
                    if (work2 != null)
                    {
                        work2.TypesAndHours.ForEach(t => typeAndHours.Add(t));
                        work2.WorkCategory = WorkCategory.PlanSecondSemester;
                        works.Add(work2);
                        work2.Teacher = teacher;
                        teacher.Works.Add(work2);
                    }
                    teachers.Add(teacher);
                });

            return Task.FromResult(new EducationData()
            {
                TeacherModels = teachers,
                TypeAndHourModels = typeAndHours,
                WorkModels = works
            });
        }

        private static void ParseData(ExcelWorksheet data,
            Action<ExcelWorksheet, EducationTeacherModel, int> jFunc)
        {
            var (teacherRows, endId) = GetTeacherRows(data);
            teacherRows.Add(endId);

            for (int i = 0; i < teacherRows.Count - 1; i++) // row
            {
                var teacherId = teacherRows[i];
                var teacherCell = data.Cells[teacherId, 2];
                if (teacherCell.Value == null) continue;
                var teacher = new EducationTeacherModel(teacherCell.Value.ToString()!);

                for (int j = teacherId + 1; j < teacherRows[i + 1]; j++) // row
                    jFunc(data, teacher, j);
            }
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

        private static List<EducationTypeAndHourModel> GetWorkData(ExcelWorksheet data, int startColumn, int valueRow)
        {
            var workData = new List<EducationTypeAndHourModel>();
            for (int i = startColumn; workData.Count != headers.Count(); i++)
            {
                var tittleCell = data.Cells[TittleRow, i];
                if (tittleCell == null || tittleCell.Value == null) continue;

                var tittle = tittleCell.Value.ToString();
                if (tittle == null || !headers.Contains(tittle)) continue;

                var valueCell = data.Cells[valueRow, i];
                var value = valueCell == null || valueCell.Value == null ? 0 : (double)valueCell.Value;
                workData.Add(new EducationTypeAndHourModel() { Key = tittle!, Value = value });
            }
            return workData;
        }

        //private static void AddIfNeed(List<EducationWorkModel> works, EducationWorkModel? work)
        //{
        //    if (work == null) return;
        //    works.Add(work);
        //}

        //private static void AddIfNeedLatest(List<EducationWorkModel> works, EducationWorkModel? work)
        //{
        //    if (work == null) return;
        //    var currentModel = works.Where(w => w.Name == work.Name).FirstOrDefault();
        //    if (currentModel == null)
        //    {
        //        works.Add(work);
        //        return;
        //    }

        //    foreach (var data in work.TypesAndHours)
        //        currentModel.TypesAndHours.Where(t => t.Key == data.Key).First().Value += data.Value;
        //}
    }
}
