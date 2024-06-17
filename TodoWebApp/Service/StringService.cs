namespace TodoWebApp.Service
{
    public class StringService
    {
        public string FormatToAC(int number)
        {
            return $"AC-{number:D4}";
        }

        public string FormatDateTime(DateTime date)
        {
            return date.ToString("d/M/yyyy");
        }
    }
}
