namespace DoiFApp.Utils
{
    public class TableDataUtil
    {
        public const string InputCommonTableHeaders =
            "лекции|" +
            "семинары|" +
            "практические занятия в группе|" +
            "практические занятия в подгруппе|" +
            "учения, д/и, круглый стол|" +
            "консультации перед экзаменами|" +
            "текущие консультации|" +
            "внеаудиторное чтение|" +
            "практика руководство|" +
            "ВКР   руководство|" +
            "курсовая работа|" +
            "проверка аудиторной контрольной работы|" +
            "проверка внеаудиторной контрольной работы|" +
            "проверка практикума, реферата|" +
            "проверка лабораторной работы|" +
            "проверка письменного зачета|" +
            "зачет устный|" +
            "экзамены|" +
            "государственная итоговая аттестация|" +
            "кандитатские экзамены|" +
            "вступительные испытания|" +
            "руководство адъюнктами";

        public const string InputCalculationTableHeaders =
            "Плановая нагрузка на начало учебного года|" +
            "Плановая аудиторная нагрузка на начало учебного года";

        public const string InputReportTableHeaders =
            "Фактическая нагрузка на начало учебного года|" +
            "Фактическая аудиторная нагрузка на начало учебного года";

        public const string OutputCommonTableHeaders =
            "чтение лекций|" +
            "проведение семинаров|" +
            "проведение практических занятий в группе|" +
            "проведение практических занятий в подгруппе|" +
            "проведение учений, деловых игр, круглых столов|" +
            "проведение консультаций перед экзаменами|" +
            "проведение текущих консультаций|" +
            "приём внеаудиторного чтения|" +
            "руководство практикой|" +
            "руководство ВКР|" +
            "руководство курсовой работой|" +
            "проверка аудиторной контрольной работы|" +
            "проверка домашней контрольной работы|" +
            "проверка практикума, реферата|" +
            "проверка лабораторной работы|" +
            "проверка письменного зачета|" +
            "приём зачетов устных|" +
            "приём экзаменов|" +
            "приём ГИА|" +
            "приём кандидатских экзаменов|" +
            "приём вступительных испытаний|" +
            "руководство адъюнктами, консультирование докторанта";

        public const string OutputCalculationTableHeaders =
            "Плановая нагрузка на начало учебного года|" +
            "Плановая аудиторная нагрузка на начало учебного года";

        public const string OutputReportTableHeaders =
            "Фактическая нагрузка общая|" +
            "Фактическая аудиторная общая";

        public static IEnumerable<string> GetHeaders(params string[] headers)
        {
            var outs = new List<string>();
            foreach (var header in headers)
                outs.AddRange(header.Split('|', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries));
            return outs;
        }

        public static string? GetEquivalent(string header)
        {
            var input1 = GetHeaders(InputCommonTableHeaders, InputCalculationTableHeaders).ToList();
            var input2 = GetHeaders(InputCommonTableHeaders, InputReportTableHeaders).ToList();
            var output1 = GetHeaders(OutputCommonTableHeaders, OutputCalculationTableHeaders).ToList();
            var output2 = GetHeaders(OutputCommonTableHeaders, InputReportTableHeaders).ToList();

            if (input1.Contains(header))
                return GetEquivalent(header, input1, output1);

            if (input2.Contains(header))
                return GetEquivalent(header, input2, output2);

            if (output1.Contains(header))
                return GetEquivalent(header, output1, input1);

            if (output2.Contains(header))
                return  GetEquivalent(header, output2, input2);

            return null;
        }

        public static string GetEquivalent(string header, List<string> fromHeaders, List<string> toHeaders)
        {
            return toHeaders[fromHeaders.IndexOf(header)];
        }
    }
}
