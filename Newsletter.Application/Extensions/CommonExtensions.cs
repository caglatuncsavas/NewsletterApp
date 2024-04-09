namespace Newsletter.Application.Extensions;
public static class CommonExtensions
{
    public static string ConvertTitleToUrl(this string str)
    {
        Dictionary<string, string> characters = new()
        {
            {"ü", "u" },
            {"ç", "c" },
            {"ğ", "g" },
            {"ı", "i" },
            {"ö", "o" },
            {"ş", "s" },
            {" ", "-" },
            {"#", "sharp" },
            {"?", "" }
        };

        string url = str.ToLower();
        foreach (var c in characters)
        {
            url = url.Replace(c.Key, c.Value);
        }

        var urls = url.Split(" ");
        url = string.Join("-", urls);

        return url;
    }
}
