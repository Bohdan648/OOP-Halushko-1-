using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";

    public override string ToString() => $"[{Id}] {Title}";
}

static class TextProcessor
{
    private static readonly HashSet<string> stopWords = new HashSet<string>
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    public static IEnumerable<string> Tokenize(string text)
    {
        return Regex.Matches(text.ToLowerInvariant(), @"[\p{L}\p{N}]+#?")
            .Select(match => match.Value);
    }

    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        return words.Where(word => !stopWords.Contains(word));
    }
}

class InvertedIndex
{
    // Слово та номери документів, у яких воно є
    private Dictionary<string, HashSet<int>> index =
        new Dictionary<string, HashSet<int>>();

    // Номер документа та сам документ
    private Dictionary<int, Document> documents =
        new Dictionary<int, Document>();

    public int DocumentCount => documents.Count;
    public int UniqueWords => index.Count;

    public void AddDocument(Document doc)
    {
        documents.Add(doc.Id, doc);

        var words = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(doc.Content));

        foreach (string word in words)
        {
            if (!index.ContainsKey(word))
                index[word] = new HashSet<int>();

            index[word].Add(doc.Id);
        }
    }

    public List<Document> SearchSingleWord(string word)
    {
        var words = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(word)).ToList();

        if (words.Count != 1 || !index.ContainsKey(words[0]))
            return new List<Document>();

        return GetDocuments(index[words[0]]);
    }

    public List<Document> SearchMultipleWords(params string[] words)
    {
        var tokens = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(string.Join(" ", words))).ToList();

        if (tokens.Count == 0 || !index.ContainsKey(tokens[0]))
            return new List<Document>();

        HashSet<int> ids = new HashSet<int>(index[tokens[0]]);

        foreach (string word in tokens)
        {
            if (!index.ContainsKey(word))
                return new List<Document>();

            ids.IntersectWith(index[word]);
        }

        return GetDocuments(ids);
    }

    public List<Document> SearchAnyWord(params string[] words)
    {
        var tokens = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(string.Join(" ", words)));

        HashSet<int> ids = new HashSet<int>();

        foreach (string word in tokens)
        {
            if (index.ContainsKey(word))
                ids.UnionWith(index[word]);
        }

        return GetDocuments(ids);
    }

    private List<Document> GetDocuments(HashSet<int> ids)
    {
        return ids.Select(id => documents[id]).ToList();
    }
}

class PerformanceTest
{
    public static List<Document> LinearSearch(
        List<Document> documents, string searchWord)
    {
        var words = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(searchWord)).ToList();

        if (words.Count != 1)
            return new List<Document>();

        return documents.Where(doc => TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(doc.Content)).Contains(words[0])).ToList();
    }

    public static void CompareSearchMethods(
        List<Document> documents, string searchWord)
    {
        InvertedIndex index = new InvertedIndex();
        Stopwatch timer = Stopwatch.StartNew();

        foreach (Document doc in documents)
            index.AddDocument(doc);

        timer.Stop();
        double buildTime = timer.Elapsed.TotalMilliseconds;

        // Перевірка результатів і прогрів перед вимірюванням
        var linear = LinearSearch(documents, searchWord);
        var indexed = index.SearchSingleWord(searchWord);

        var linearIds = new HashSet<int>(linear.Select(doc => doc.Id));

        if (!linearIds.SetEquals(indexed.Select(doc => doc.Id)))
            throw new Exception("Результати пошуку не збігаються.");

        const int repeats = 100;

        timer.Restart();

        for (int i = 0; i < repeats; i++)
            linear = LinearSearch(documents, searchWord);

        timer.Stop();
        double linearTime = timer.Elapsed.TotalMilliseconds / repeats;

        timer.Restart();

        for (int i = 0; i < repeats; i++)
            indexed = index.SearchSingleWord(searchWord);

        timer.Stop();
        double indexTime = timer.Elapsed.TotalMilliseconds / repeats;

        Console.WriteLine($"| {documents.Count} | {linearTime:F4} | " +
            $"{indexTime:F4} | {buildTime:F4} | {linear.Count}/{indexed.Count} |");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<Document> documents = new List<Document>
        {
            new Document
            {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern object-oriented programming language developed by Microsoft."
            },
            new Document
            {
                Id = 2,
                Title = "Java Basics",
                Content = "Java is a popular programming language known for its portability and performance."
            },
            new Document
            {
                Id = 3,
                Title = "Python Overview",
                Content = "Python is a high-level programming language with simple syntax."
            },
            new Document
            {
                Id = 4,
                Title = "C# Advanced Features",
                Content = "C# supports LINQ, async/await, and many modern programming paradigms."
            },
            new Document
            {
                Id = 5,
                Title = "Comparing Languages",
                Content = "C#, Java, and Python are among the most popular programming languages today."
            }
        };

        InvertedIndex index = new InvertedIndex();

        foreach (Document doc in documents)
            index.AddDocument(doc);

        Console.WriteLine("Документів: " + index.DocumentCount);
        Console.WriteLine("Унікальних слів: " + index.UniqueWords);

        PrintResults("Одне слово: C#", index.SearchSingleWord("C#"));

        PrintResults("AND: c# async",
            index.SearchMultipleWords("c#", "async"));

        PrintResults("OR: java python",
            index.SearchAnyWord("java", "python"));

        Console.WriteLine(
            "\nПошук C#: середній час одного запиту зі 100 повторень.");

        Console.WriteLine(
            "| Документів | Лінійний, мс | Індекс, мс | Побудова, мс | Знайдено лінійно/індекс |");

        Console.WriteLine("|---|---|---|---|---|");

        foreach (int count in new int[] { 10, 1000 })
        {
            List<Document> generated = new List<Document>();

            for (int i = 0; i < count; i++)
            {
                Document original = documents[i % documents.Count];

                generated.Add(new Document
                {
                    Id = i + 1,
                    Title = original.Title,
                    Content = original.Content
                });
            }

            PerformanceTest.CompareSearchMethods(generated, "C#");
        }
    }

    static void PrintResults(string title, List<Document> results)
    {
        Console.WriteLine("\n" + title);

        foreach (Document doc in results.OrderBy(doc => doc.Id))
            Console.WriteLine(doc);
    }
}