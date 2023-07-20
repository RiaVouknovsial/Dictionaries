using System.Text.Json;
using System.Text;

class RussianUkrainianDictionary : Dictionary
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
            Console.WriteLine("Словарь успешно сохранен в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при сохранении словаря в файл: " + ex.Message);
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
                Console.WriteLine("Словарь успешно загружен из файла.");
            }
            else
            {
                Console.WriteLine("Файл словаря не найден. Используется пустой словарь.");
                dictionary = new Dictionary<string, List<string>>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при загрузке словаря из файла: " + ex.Message);
        }
    }
    public override void AddWord()
    {
        Console.Write("Введите русское слово: ");
        string word = Console.ReadLine().ToLower();

        if (dictionary.ContainsKey(word))
        {
            Console.WriteLine("Слово уже существует в словаре.");
            return;
        }

        Console.WriteLine("Введите перевод(ы) слова (введите 'готово', чтобы закончить):");

        List<string> translations = new List<string>();
        string translation = "";

        while (translation != "готово")
        {
            translation = Console.ReadLine();
            if (translation != "готово")
                translations.Add(translation);
        }

        dictionary[word] = translations;
        Console.WriteLine("Слово успешно добавлено в словарь.");

        SaveDictionaryToFile("dictionaryRusUkr.json");
        Console.ReadLine();
    }


    public override void ReplaceWord()
    {
        Console.Write("Введите слово или перевод, который нужно заменить: ");
        string searchTerm = Console.ReadLine();

        var matches = dictionary.Where(x => x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("Слово или перевод не найдены в словаре.");
            Console.ReadLine();
            return;
        }

        Console.Write("Введите новое значение:");
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

        Console.WriteLine("Замена выполнена успешно.");
        SaveDictionaryToFile("dictionaryRusUkr.json");
        Console.ReadLine();
    }
    public override void RemoveWord()
    {
        Console.Write("Введите слово или перевод, который нужно удалить: ");
        string searchTerm = Console.ReadLine();

        var matches = dictionary.Where(x => x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                             x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("Слово или перевод не найдены в словаре.");
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
                    Console.WriteLine("Нельзя удалить перевод слова, если это последний вариант перевода.");
                    Console.ReadLine();
                    return;
                }
            }
        }

        Console.WriteLine("Удаление выполнено успешно.");
        SaveDictionaryToFile("dictionaryRusUkr.json");
        Console.ReadLine();
    }

    public override void SearchTranslation()
    {
        Console.Write("Введите слово для поиска перевода: ");
        string searchTerm = Console.ReadLine().ToLower();

        var results = dictionary.Where(x =>
            x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("Перевод не найден в словаре.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Результаты поиска:");

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
            string json = File.ReadAllText(filename, Encoding.Default);
            dictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

            Console.WriteLine("Русско-украинский словарь:");

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
            Console.WriteLine("Ошибка при просмотре файла: " + ex.Message);
        }
        Console.ReadLine();
    }
    public override void ExportDictionaryToTxt(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.Default))
            {
                foreach (var entry in dictionary.OrderBy(x => x.Key))
                {
                    string word = entry.Key;
                    List<string> translations = entry.Value;
                    string translationsText = string.Join(", ", translations);
                    writer.WriteLine($"\"{word}\": \"{translationsText}\"");
                }
            }

            Console.WriteLine("Словарь успешно экспортирован в файл txt.");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при экспорте словаря в файл txt: " + ex.Message);
        }
        Console.ReadLine();
    }
}