using System.Text.Json;
using System.Text;

class UkrainianEnglishDictionary : Dictionary
{
    public Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();

    public override void SaveDictionaryToFile(string filePath)
    {
        try
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(dictionary, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
            Console.WriteLine("Словник успішно збережено у файлі.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка під час зберігання словника у файлі: " + ex.Message);
        }
        Console.ReadLine();
    }

    public override void LoadDictionaryFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);
                dictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
                Console.WriteLine("Словник успішно завантажено з файла.");
            }
            else
            {
                Console.WriteLine("Файл словника не знайдено. Використовується порожній словник.");
                dictionary = new Dictionary<string, List<string>>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка під час завантаження словника з файла: " + ex.Message);
        }
    }

    public override void AddWord()
    {
        Console.Write("Введіть українське слово: ");
        string word = Console.ReadLine().ToLower();

        if (dictionary.ContainsKey(word))
        {
            Console.WriteLine("Слово вже присутнє в словнику.");
            return;
        }

        Console.WriteLine("Введіть переклад(и) слова (введіть 'готово', щоб завершити):");

        List<string> translations = new List<string>();
        string translation = "";

        while (translation != "готово")
        {
            translation = Console.ReadLine();
            if (translation != "готово")
                translations.Add(translation);
        }

        dictionary[word] = translations;
        Console.WriteLine("Слово успішно додано в словник.");

        SaveDictionaryToFile("dictionaryUkrEng.json");
        Console.ReadLine();
    }

    public override void ReplaceWord()
    {
        Console.Write("Введіть слово або переклад, який треба замінити: ");
        string searchTerm = Console.ReadLine();

        var matches = dictionary.Where(x => x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("Слово або переклад не знайдені в словнику.");
            return;
        }

        Console.Write("Введіть нове значення: ");
        string newValue = Console.ReadLine();

        foreach (var match in matches)
        {
            string word = match.Key;
            List<string> translations = match.Value;

            if (word.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                dictionary.Remove(word);
                dictionary[newValue] = translations;
            }

            for (int i = 0; i < translations.Count; i++)
            {
                if (translations[i].Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    translations[i] = newValue;
                }
            }
        }

        Console.WriteLine("Заміну виконано успішно.");
        SaveDictionaryToFile("dictionaryUkrEng.json");
        Console.ReadLine();
    }
    public override void RemoveWord()
    {
        Console.Write("Введіть слово або переклад, який потрібно видалити: ");
        string searchTerm = Console.ReadLine();

        var matches = dictionary.Where(x => x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                             x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("Слово або переклад не знайдені в словнику.");
            return;
        }

        foreach (var match in matches)
        {
            string word = match.Key;
            List<string> translations = match.Value;

            if (word.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                dictionary.Remove(word);
            }
            else
            {
                translations.RemoveAll(t => t.Equals(searchTerm, StringComparison.OrdinalIgnoreCase));

                if (translations.Count == 0)
                {
                    Console.WriteLine("Не можна видалити переклад слова, якщо це останній варіант перекладу.");
                    Console.ReadLine();
                    return;
                }
            }
        }

        Console.WriteLine("Видалення виконано успішно.");
        SaveDictionaryToFile("dictionaryUkrEng.json");
        Console.ReadLine();
    }

    public override void SearchTranslation()
    {
        Console.Write("Введіть слово для пошуку перекладу: ");
        string searchTerm = Console.ReadLine().ToLower();

        var results = dictionary.Where(x =>
            x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("Переклад не знайдено в словнику.");
            return;
        }

        Console.WriteLine("Результати пошуку:");

        foreach (var entry in results)
        {
            string word = entry.Key;
            List<string> translations = entry.Value;
            Console.WriteLine($"\"{word}\": {string.Join(", ", translations)}");
        }
        Console.ReadLine();
    }
    public override void ViewFileContent(string filename)
    {
        try
        {
            string json = File.ReadAllText(filename);
            dictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

            Console.WriteLine("Українсько-англійський словник:");

            foreach (var entry in dictionary.OrderBy(x => x.Key))
            {
                string word = entry.Key;
                List<string> translations = entry.Value;
                string translationsText = string.Join(", ", translations);
                Console.WriteLine($"\"{word}\": \"{translationsText}\"");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка під час перегляду файла: " + ex.Message);
        }
        Console.ReadLine();
    }
    public override void ExportDictionaryToTxt(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var entry in dictionary.OrderBy(x => x.Key))
                {
                    string word = entry.Key;
                    List<string> translations = entry.Value;
                    string translationsText = string.Join(", ", translations);
                    writer.WriteLine($"\"{word}\": \"{translationsText}\"");
                }
            }

            Console.WriteLine("Словник успішно експортовано у файл txt.");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка під час експорту словника у файл txt.: " + ex.Message);
        }
        Console.ReadLine();
    }

}