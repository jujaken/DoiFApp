namespace DoiFApp.Services
{
    public class CaseComparator : ICaseComparator
    {
        public string GetName(string fullname)
            => fullname[1] switch
            {
                '1' => "Волгина",
                '2' => "Волгина",
                '3' => "Волгина",
                '5' => "Коптево",
                _ => "Другие площадки",
            };
    }
}
