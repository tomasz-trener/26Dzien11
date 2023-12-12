using System.Net;
using System.Text.RegularExpressions;


string regexTemplate = "<div class=\"BNeawe iBp4i AP7Wnd\">(-{0,1}\\d{1,3}).[CF]<\\/div>";


while (true)
{
    Console.WriteLine("Podaj nazwe miasta");
    string city = Console.ReadLine();
    string url = $"https://www.google.com/search?q=pogoda {city}";

    WebClient wc = new WebClient();
    string data = wc.DownloadString(url);

    File.WriteAllText("pogoda.html", data);

    try
    {
        Regex rx = new Regex(regexTemplate);
        Match match = rx.Match(data);

        string result = match.Groups[1].Value;

        Console.WriteLine(result);
    }
    catch (Exception)
    {
        Console.WriteLine("Nie udało się pobrać temperatury");
        continue;
    }
}
