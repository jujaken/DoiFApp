using DoiFApp.Data;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using OfficeOpenXml;

namespace DoiFApp.Services.Excel
{
    public class ExcelEducationReader(AppDbContext context, IRepo<EducationTeacherModel> teacherRepo, IRepo<EducationWorkModel> workRepo) : IEducationReader
    {
        private readonly AppDbContext context = context;
        private readonly IRepo<EducationTeacherModel> teacherRepo = teacherRepo;
        private readonly IRepo<EducationWorkModel> workRepo = workRepo;

        private const int TittleRow = 8;
        private const string WorkStr = "лекции\tсеминары\tпрактические занятия в группе\tпрактические занятия в подгруппе\tучения, д/и, круглый стол\tконсультации перед экзаменами\tтекущие консультации\tвнеаудиторное чтение\tпрактика руководство\tВКР   руководство\tкурсовая работа\tконтрольная работа аудиторная\tконтрольная работа домашняя\tпроверка практикума, реферата\tпроверка лабораторной работы\tзащита практики\tзачет устный\tзачет письменный\tвступительные испытания\tэкзамены\tгосударственные экзамены\tвступительные и кандитатские экзамены (адъюнктура)\tруководство адъюнктами";
        private static readonly IEnumerable<string> workStrSplit = WorkStr.Split('\t', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

        public Task ReadFromFile(string fileName)
        {
            context.RecreateEducation();

            using var package = new ExcelPackage(fileName);
            ParseData(package.Workbook.Worksheets["Расчет"],
                (data, teacher, j) =>
                {
                    var work1 = GetWorkTeacher(data, 15, 2, j);
                    AddIfNeed(teacher.Works1, work1);
                    var work2 = GetWorkTeacher(data, 15, 2, j);
                    AddIfNeed(teacher.Works2, work2);
                },
                (data, teacher, i) =>
                {
                    teacher.Works1.ForEach(w => workRepo.Create(w));
                    teacher.Works2.ForEach(w => workRepo.Create(w));
                    teacherRepo.Create(teacher);
                });

            return Task.FromResult(0);
        }

        private void ParseData(ExcelWorksheet data,
            Action<ExcelWorksheet, EducationTeacherModel, int> jFunc,
            Action<ExcelWorksheet, EducationTeacherModel, int> iFunc)
        {
            var (teacherRows, endId) = GetTeacherRows(data);
            teacherRows.Add(endId);

            for (int i = 0; i < teacherRows.Count - 1; i++) // row
            {
                var teacherId = teacherRows[i];
                var teacherCell = data.Cells[teacherId, 2];
                var teacher = new EducationTeacherModel(teacherCell.Value.ToString()!);

                for (int j = teacherId + 1; j < teacherRows[i + 1]; j++) // row
                    jFunc(data, teacher, j);
                iFunc(data, teacher, i);
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
            for (int i = startColumn; workData.Count != workStrSplit.Count(); i++)
            {
                var tittleCell = data.Cells[TittleRow, i];
                if (tittleCell == null || tittleCell.Value == null) continue;

                var tittle = tittleCell.Value.ToString();
                if (!workStrSplit.Contains(tittle)) continue;

                var valueCell = data.Cells[valueRow, i];
                var value = valueCell == null || valueCell.Value == null ? 0 : (double)valueCell.Value;
                workData.Add(new EducationTypeAndHourModel() { Key = tittle!, Value = value });
            }
            return workData;
        }

        private static void AddIfNeed(List<EducationWorkModel> works, EducationWorkModel? work)
        {
            if (work == null) return;
            var currentModel = works.Where(w => w.Name == work.Name).FirstOrDefault();
            if (currentModel == null)
            {
                works.Add(work);
                return;
            }

            foreach (var data in work.TypesAndHours)
                currentModel.TypesAndHours.Where(t => t.Key == data.Key).First().Value += data.Value;
        }
    }
}
