public static class Helpers
{
    public static string CenterText(string text, int width) 
    {
        if (text.Length >= width) return text.Substring(0, width);
        int leftPadding = (width - text.Length) / 2;
        int rightPadding = width - text.Length - leftPadding;
        return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }

    public static string TruncateString(string str, int maxLength)
    {
        if (string.IsNullOrEmpty(str)) return str;
        if (str.Length <= maxLength) return str;
        return str.Substring(0, maxLength - 3) + "...";
    }

    public static string GetDaysWord(int days)
    {
        if (days % 10 == 1 && days % 100 != 11)
            return "день";
        else if (days % 10 >= 2 && days % 10 <= 4 && (days % 100 < 10 || days % 100 >= 20))
            return "дня";
        else
            return "дней";
    }
}