using System.Text.Json;
using System.Text;

class EnglishRussianDictionary : Dictionary
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
            Console.WriteLine("The dictionary was successfully saved to a file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving dictionary to file: " + ex.Message);
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
                Console.WriteLine("The dictionary was successfully loaded from the file.");
            }
            else
            {
                Console.WriteLine("Dictionary file not found. An empty dictionary is used.");
                dictionary = new Dictionary<string, List<string>>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading dictionary from file: " + ex.Message);
        }
    }

    public override void AddWord()
    {
        Console.Write("Enter an English word: ");
        string word = Console.ReadLine().ToLower();

        if (dictionary.ContainsKey(word))
        {
            Console.WriteLine("The word already exists in the dictionary.");
            return;
        }

        Console.WriteLine("Enter the translation(s) of the word (type 'done' to complete):");

        List<string> translations = new List<string>();
        string translation = "";

        while (translation != "done")
        {
            translation = Console.ReadLine();
            if (translation != "done")
                translations.Add(translation);
        }

        dictionary[word] = translations;
        Console.WriteLine("The word has been successfully added to the dictionary.");

        SaveDictionaryToFile("dictionaryEngRus.json");
        Console.ReadLine();
    }

    public override void ReplaceWord()
    {
        Console.Write("Enter the word or translation to be replaced: ");
        string searchTerm = Console.ReadLine();

        var matches = dictionary.Where(x => x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("The word or translation was not found in the dictionary.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter a new value:");
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

        Console.WriteLine("Replacement completed successfully.");
        SaveDictionaryToFile("dictionaryEngRus.json");
        Console.ReadLine();
    }
    public override void RemoveWord()
    {
        Console.Write("Enter the word or translation you want to remove:");
        string searchTerm = Console.ReadLine();

        var matches = dictionary.Where(x => x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                             x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (matches.Count == 0)
        {
            Console.WriteLine("Word or translation not found in dictionary.");
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
                    Console.WriteLine("You cannot delete the translation of a word if it is the latest translation.");
                    Console.ReadLine();
                    return;
                }
            }
        }

        Console.WriteLine("Removal completed successfully.");
        SaveDictionaryToFile("dictionaryEngRus.json");
        Console.ReadLine();
    }

    public override void SearchTranslation()
    {
        Console.Write("Enter a word to search for a translation: ");
        string searchTerm = Console.ReadLine().ToLower();

        var results = dictionary.Where(x =>
            x.Key.Equals(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            x.Value.Any(v => v.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("Translation not found in the dictionary.");
            return;
        }

        Console.WriteLine("Searching results:");
        Console.ReadLine();
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

            Console.WriteLine("English-Russian dictionary:");

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
            Console.WriteLine("Error while viewing file: " + ex.Message);
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

            Console.WriteLine("Dictionary successfully exported to txt file.");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error exporting a dictionary to a txt file: " + ex.Message);
        }
        Console.ReadLine();
    }
}
