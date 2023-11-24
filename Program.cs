

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

public class UserRecord
{
    public string Name { get; set; }
    public int CharactersPerMinute { get; set; }
    public int CharactersPerSecond { get; set; }
}

public static class RecordsTable
{
    private static List<UserRecord> _records = new List<UserRecord>();

    public static void AddRecord(UserRecord record)
    {
        _records.Add(record);
    }

    public static List<UserRecord> LoadTable(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<UserRecord>>(json);
    }

    public static void SaveTable(string filePath)
    {
        string json = JsonSerializer.Serialize(_records);
        File.WriteAllText(filePath, json);
    }

    public static void PrintTable()
    {
        Console.WriteLine("Таблица рекордов:");
        Console.WriteLine("------------------");
        Console.WriteLine("| Имя     | Символов в минуту | Символов в секунду |");
        Console.WriteLine("------------------");

        foreach (var record in _records)
        {
            Console.WriteLine($"| {record.Name,-7} | {record.CharactersPerMinute,-18} | {record.CharactersPerSecond,-20} |");
        }

        Console.WriteLine("------------------");
    }
}

public class TypingTest
{
    private string _text;

    public TypingTest()
    {
        _text = GenerateText();
    }

    public void Start()
    {
        Console.Write("Введите свое имя: ");
        string name = Console.ReadLine();

        Console.WriteLine($"Напечатайте следующий текст: {_text}");

        Stopwatch stopwatch = Stopwatch.StartNew();
        string input = Console.ReadLine();
        stopwatch.Stop();

        int charactersTyped = input.Length;
        double timeInSeconds = stopwatch.Elapsed.TotalSeconds;

        int charactersPerMinute = (int)(charactersTyped / (timeInSeconds / 60));
        int charactersPerSecond = (int)(charactersTyped / timeInSeconds);

        UserRecord record = new UserRecord { Name = name, CharactersPerMinute = charactersPerMinute, CharactersPerSecond = charactersPerSecond };
        RecordsTable.AddRecord(record);

        RecordsTable.PrintTable();
    }

    private string GenerateText()
    {
        return "В далекой Африке, на реке Замбези, есть настоящее чудо\r\nприроды – грандиозный водопад Виктория. Его открыл в 1855 г.\r\nанглийский путешественник Давид Ливингстон и назвал в честь\r\nанглийской королевы. Ширина этого водопада составляет 1800\r\nметров, а высота – 120 метров. Шум воды слышен за 25\r\nкилометров, а еще дальше, километров за 40, видно высокое\r\nоблако водяной пыли. Радуга играет в брызгах воды, множество\r\nручейков стекает с противоположной водопаду стены каньона, и\r\nвсе новые каскады воды обрушиваются на них, увлекая за собой..";
    }

    public static class Program
    {
        private static readonly string _filePath = "records.json";

        public static void Main()
        {
            if (File.Exists(_filePath))
            {
                RecordsTable.LoadTable(_filePath);
            }

            while (true)
            {
                TypingTest typingTest = new TypingTest();
                typingTest.Start();

                RecordsTable.SaveTable(_filePath);

                Console.Write("Хотите пройти тест еще раз? (y/n) ");
                string answer = Console.ReadLine();

                if (answer.ToLower() != "y")
                {
                    break;
                }
            }
        }
    }
}


