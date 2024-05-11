using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

class Szyfrowanie_Vigenere
{
    static void Main()
    {
        string Ścieżka = @"C:\sss"; // Ścieżka do folderu z plikami tekstowymi
        string NazwaPlikuKlucz = Path.Combine(Ścieżka, "klucz.txt");
        string NazwaPlikuTekst = Path.Combine(Ścieżka, "tekst.txt");

        if (!File.Exists(NazwaPlikuKlucz) || !File.Exists(NazwaPlikuTekst))
        {
            Console.WriteLine($"Brak pliku {NazwaPlikuKlucz} lub {NazwaPlikuTekst} w folderze projektu.");
            return;
        }

        string key = File.ReadAllText(NazwaPlikuKlucz).ToUpper();
        string text = File.ReadAllText(NazwaPlikuTekst);

        while (true)
        {
            Console.WriteLine("Wybierz co chcesz zrobić:");
            Console.WriteLine("1. Szyfrowanie i zapisanie do pliku");
            Console.WriteLine("2. Deszyfrowanie i zapisanie do pliku");
            Console.WriteLine("3. Kryptoanaliza");
            Console.WriteLine("4. Wyjście");
            Console.WriteLine("Autor: Hintz Adam");
            
            Console.WriteLine("");
            Console.Write("Twój wybór: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    string ZaszyfrowanyTekst = Szyfruj(text, key);
                    File.WriteAllText(NazwaPlikuTekst, ZaszyfrowanyTekst, Encoding.UTF8);
                    Console.WriteLine("Zaszyfrowany tekst został zapisany w pliku 'tekst.txt'.");
                    break;
                case "2":
                    text = File.ReadAllText(NazwaPlikuTekst); // Wczytaj zaszyfrowany tekst z pliku
                    string OdzyfrowanyTekst = Deszyfruj(text, key);
                    File.WriteAllText(NazwaPlikuTekst, OdzyfrowanyTekst, Encoding.UTF8);
                    Console.WriteLine("Odszyfrowany tekst został zapisany w pliku 'tekst.txt'.");
                    break;
                case "3":
                    Console.WriteLine("Próba kryptoanalizy...");
                    Kryptoanaliza(text, NazwaPlikuTekst);
                    Console.WriteLine("Rozszyfrowany tekst został zapisany w pliku 'rozszfrowany_tekst.txt'.");
                    break;
                case "4":
                    Console.WriteLine("Zamykanie programu.");
                    return;
                default:
                    Console.WriteLine("Niepoprawny wybór. Wybierz ponownie.");
                    break;
            }
        }
    }

    static string Szyfruj(string text, string key)
    {
        StringBuilder ZaszyfrowanyTekst = new StringBuilder();
        int keyIndex = 0;

        foreach (char Litera in text)
        {
            if (char.IsLetter(Litera))
            {
                int shift = char.ToUpper(key[keyIndex]) - 'A';
                char Zaszyfrowanalitera = (char)((((char.ToUpper(Litera) + shift) - 'A') % 26) + 'A');
                if (char.IsLower(Litera))
                {
                    Zaszyfrowanalitera = char.ToLower(Zaszyfrowanalitera); // Zachowaj małą literę, jeśli oryginalna była mała
                }
                ZaszyfrowanyTekst.Append(Zaszyfrowanalitera);
                keyIndex = (keyIndex + 1) % key.Length;
            }
            else
            {
                ZaszyfrowanyTekst.Append(Litera);
            }
        }

        return ZaszyfrowanyTekst.ToString();
    }

    static string Deszyfruj(string text, string key)
    {
        StringBuilder OdzyfrowanyTekst = new StringBuilder();
        int keyIndex = 0;

        foreach (char Litera in text)
        {
            if (char.IsLetter(Litera))
            {
                int shift = char.ToUpper(key[keyIndex]) - 'A';
                char OdszyfrowanaLitera = (char)((((char.ToUpper(Litera) - shift) - 'A' + 26) % 26) + 'A');
                if (char.IsLower(Litera))
                {
                    OdszyfrowanaLitera = char.ToLower(OdszyfrowanaLitera);
                }
                OdzyfrowanyTekst.Append(OdszyfrowanaLitera);
                keyIndex = (keyIndex + 1) % key.Length;
            }
            else
            {
                OdzyfrowanyTekst.Append(Litera);
            }
        }

        return OdzyfrowanyTekst.ToString();
    }

    static void Kryptoanaliza(string text, string textFileName)
    {
        Dictionary<char, int> WystępowanieLiter = new Dictionary<char, int>();

        // Zliczanie wystąpień każdej litery w tekście
        foreach (char letter in text)
        {
            if (char.IsLetter(letter))
            {
                char DużaLitera = char.ToUpper(letter);
                if (WystępowanieLiter.ContainsKey(DużaLitera))
                {
                    WystępowanieLiter[DużaLitera]++;
                }
                else
                {
                    WystępowanieLiter[DużaLitera] = 1;
                }
            }
        }

       
        var sortedFrequency = WystępowanieLiter.OrderByDescending(pair => pair.Value);

        // Znalezienie najczęściej występującej litery (przyjęcie, że jest to litera E)
        char CzęstaLitera = sortedFrequency.First().Key;

        // Obliczenie przesunięcia, aby odszyfrować tekst
        int CzęstaLiteraShift = CzęstaLitera - 'E';

        
        StringBuilder OdzyfrowanyTekst = new StringBuilder();
        foreach (char letter in text)
        {
            if (char.IsLetter(letter))
            {
                char decryptedLetter = (char)((((char.ToUpper(letter) - CzęstaLiteraShift) - 'A' + 26) % 26) + 'A');
                if (char.IsLower(letter))
                {
                    decryptedLetter = char.ToLower(decryptedLetter);
                }
                OdzyfrowanyTekst.Append(decryptedLetter);
            }
            else
            {
                OdzyfrowanyTekst.Append(letter);
            }
        }

        // Zapisanie odszyfrowanego tekstu do nowego pliku
        string OdszyfrowanyPlik = Path.Combine(Path.GetDirectoryName(textFileName), "rozszfrowany_tekst.txt");
        File.WriteAllText(OdszyfrowanyPlik, OdzyfrowanyTekst.ToString(), Encoding.UTF8);
    }
}
