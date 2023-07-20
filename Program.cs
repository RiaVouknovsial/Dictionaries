//Создать приложение «Словари».
//Основная задача проекта: хранить словари на разных языках и разрешать пользователю
//находить перевод нужного слова или фразы.
//Интерфейс приложения должен предоставлять такие возможности:
//■ Создавать словарь. При создании нужно указать тип словаря.
//Например, англо-русский или русско-английский; англо-украинский или украинско-английский, украинско-русский или русско-украинский
//■ Добавлять слово и его перевод в уже существующий словарь c клавиатуры. Так как у слова может быть
//несколько переводов, необходимо поддерживать возможность создания нескольких вариантов перевода.
//■ Заменять слово или его перевод в словаре.
//■ Удалять слово или перевод. Если удаляется слово, все его переводы удаляются вместе с ним.
//Нельзя удалить перевод слова, если это последний вариант перевода.
//■ Искать перевод слова.
//■ Словари должны храниться в файлах.
//■ Слово и варианты его переводов можно экспортировать в отдельный файл результата.
//■ При старте программы необходимо показывать меню для работы с программой.
//Если выбор пункта меню открывает подменю, то тогда в нем требуется предусмотреть
//возможность возврата в предыдущее меню.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;


        Console.OutputEncoding = Encoding.UTF8;

        var englishRussianDictionary = new EnglishRussianDictionary();
        var russianUkrainianDictionary = new RussianUkrainianDictionary();
        var ukrainianEnglishDictionary = new UkrainianEnglishDictionary();

        string englishRussianFilename = "dictionaryEngRus.json";
        englishRussianDictionary.LoadDictionaryFromFile(englishRussianFilename);

        string russianUkrainianFilename = "dictionaryRusUkr.json";
        russianUkrainianDictionary.LoadDictionaryFromFile(russianUkrainianFilename);

        string ukrainianEnglishFilename = "dictionaryUkrEng.json";
        ukrainianEnglishDictionary.LoadDictionaryFromFile(ukrainianEnglishFilename);


        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Меню:");
            Console.WriteLine("Выберите словарь:");
            Console.WriteLine("1. English-Russian dictionary");
            Console.WriteLine("2. Русско-украинский словарь");
            Console.WriteLine("3. Українсько-англійський словник");
            Console.WriteLine("4. Выход из программы");
            string dictionaryChoice = Console.ReadLine();

            switch (dictionaryChoice)
            {
                case "1":
                    DictionaryMenu(englishRussianDictionary);
                    break;
                case "2":
                    DictionaryMenu2(russianUkrainianDictionary);
                    break;
                case "3":
                    DictionaryMenu3(ukrainianEnglishDictionary);
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Некорректный выбор словаря.");
                    break;
            }
        }

        static void DictionaryMenu(EnglishRussianDictionary dictionaryEngRus)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("English-Russian dictionary menu:");
                Console.WriteLine("1. Add word to dictionary");
                Console.WriteLine("2. Replace a word or translation in a dictionary");
                Console.WriteLine("3. Remove word from dictionary");
                Console.WriteLine("4. Finding a translation of a word");
                Console.WriteLine("5. View dictionary");
                Console.WriteLine("6. Export dictionary");
                Console.WriteLine("7. Back to main menu");
                Console.Write("Select a menu item: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        dictionaryEngRus.AddWord();
                        break;
                    case "2":
                        dictionaryEngRus.ReplaceWord();
                        break;
                    case "3":
                        dictionaryEngRus.RemoveWord();
                        break;
                    case "4":
                        dictionaryEngRus.SearchTranslation();
                        break;
                    case "5":
                        dictionaryEngRus.ViewFileContent("dictionaryEngRus.json");
                        break;
                    case "6":
                        Console.Write("Enter the path to the txt file: ");
                        string txtFilePath = Console.ReadLine();
                        dictionaryEngRus.ExportDictionaryToTxt(txtFilePath);
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Incorrect choice. Please select an item from the menu.");
                        break;
                }
            }
        }

        static void DictionaryMenu2(RussianUkrainianDictionary dictionaryRusUkr)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Меню русско-украинского словаря:");
                Console.WriteLine("1. Добавить слово в словарь");
                Console.WriteLine("2. Заменить слово или перевод в словаре");
                Console.WriteLine("3. Удалить слово из словаря");
                Console.WriteLine("4. Поиск перевода слова");
                Console.WriteLine("5. Просмотреть словарь");
                Console.WriteLine("6. Экспортировать словарь");
                Console.WriteLine("7. Вернуться в главное меню");
                Console.Write("Выберите пункт меню: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        dictionaryRusUkr.AddWord();
                        break;
                    case "2":
                        dictionaryRusUkr.ReplaceWord();
                        break;
                    case "3":
                        dictionaryRusUkr.RemoveWord();
                        break;
                    case "4":
                        dictionaryRusUkr.SearchTranslation();
                        break;
                    case "5":
                        dictionaryRusUkr.ViewFileContent("dictionaryRusUkr.json");
                        break;
                    case "6":
                        Console.Write("Введите путь к файлу txt: ");
                        string txtFilePath = Console.ReadLine();
                        dictionaryRusUkr.ExportDictionaryToTxt(txtFilePath);
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, выберите пункт из меню.");
                        break;
                }
            }
        }

        static void DictionaryMenu3(UkrainianEnglishDictionary dictionaryUkrEng)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Меню українсько-англійського словника:");
                Console.WriteLine("1. Додати слово в словник");
                Console.WriteLine("2. Замінити слово або переклад в словнику");
                Console.WriteLine("3. Видалити слово із словника");
                Console.WriteLine("4. Пошук перекладу слова");
                Console.WriteLine("5. Переглянути словник");
                Console.WriteLine("6. Експортувати словник");
                Console.WriteLine("7. Повернутися до головного меню");
                Console.Write("Виберіть пункт меню: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        dictionaryUkrEng.AddWord();
                        break;
                    case "2":
                        dictionaryUkrEng.ReplaceWord();
                        break;
                    case "3":
                        dictionaryUkrEng.RemoveWord();
                        break;
                    case "4":
                        dictionaryUkrEng.SearchTranslation();
                        break;
                    case "5":
                        dictionaryUkrEng.ViewFileContent("dictionaryUkrEng.json");
                        break;
                    case "6":
                        Console.Write("Введіть шлях до файлу txt: ");
                        string txtFilePath = Console.ReadLine();
                        dictionaryUkrEng.ExportDictionaryToTxt(txtFilePath);
                        break;
                    case "7":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неправильний вибір. Будь ласка, виберіть пункт з меню.");
                        break;
                }
            }
        }
    

