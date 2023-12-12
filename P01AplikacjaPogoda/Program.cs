using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P01AplikacjaPogoda
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string szukanyZnak = "°";
            string znakKoncowy = ">";

            while (true)
            {
                Console.WriteLine("Podaj nazwe miasta");
                string miasto = Console.ReadLine();
                string adres = $"https://www.google.com/search?q=pogoda {miasto}";

                WebClient wc = new WebClient();
                string dane = wc.DownloadString(adres);

                File.WriteAllText("pogoda.html", dane);

                try
                {
                    int indx = dane.IndexOf(szukanyZnak);
                    int aktualnaPozycja = indx;
                    int liczbaIteracji = 0;

                    while (dane.Substring(aktualnaPozycja, 1) != znakKoncowy)
                    {
                        liczbaIteracji++;
                        aktualnaPozycja--;
                        if (liczbaIteracji > 4)
                            throw new Exception();
                    }


                    string wynik = dane.Substring(aktualnaPozycja + 1, indx - aktualnaPozycja + 1);
                    Console.WriteLine(wynik);
                }
                catch (Exception)
                {
                    Console.WriteLine("Nie udało się pobrać temperatury");
                    continue;
                }


            }
        }
    }
}
