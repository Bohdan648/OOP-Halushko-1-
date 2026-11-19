using System;
using System.Collections.Generic;
using System.Text;

class FileMetadata
{
    public string FileName { get; set; } = "";
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = "";
}

class FileMetadataCache
{
    private Dictionary<string, FileMetadata> cache =
        new Dictionary<string, FileMetadata>();

    public void Add(FileMetadata file)
    {
        cache[file.FileName] = file;
    }

    public FileMetadata? Get(string name)
    {
        if (cache.ContainsKey(name))
            return cache[name];

        return null;
    }

    public void Remove(string name)
    {
        cache.Remove(name);
    }

    public bool Contains(string name)
    {
        return cache.ContainsKey(name);
    }
}

class TagManager
{
    private Dictionary<string, HashSet<string>> articleTags =
        new Dictionary<string, HashSet<string>>();

    public void AddTag(string id, string tag)
    {
        if (!articleTags.ContainsKey(id))
        {
            articleTags[id] =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        articleTags[id].Add(tag);
    }

    public void RemoveTag(string id, string tag)
    {
        if (articleTags.ContainsKey(id))
            articleTags[id].Remove(tag);
    }

    // Отримуємо копію тегів статті
    public HashSet<string> GetTags(string id)
    {
        HashSet<string> tags =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (articleTags.ContainsKey(id))
            tags.UnionWith(articleTags[id]);

        return tags;
    }

    public HashSet<string> GetAllUniqueTags()
    {
        HashSet<string> result =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (HashSet<string> tags in articleTags.Values)
            result.UnionWith(tags);

        return result;
    }

    public HashSet<string> FindCommonTags(string id1, string id2)
    {
        HashSet<string> result = GetTags(id1);
        result.IntersectWith(GetTags(id2));
        return result;
    }

    public HashSet<string> FindDifferentTags(string id1, string id2)
    {
        HashSet<string> result = GetTags(id1);
        result.ExceptWith(GetTags(id2));
        return result;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("=== Кеш метаданих ===");

        FileMetadataCache cache = new FileMetadataCache();

        FileMetadata file = new FileMetadata();
        file.FileName = "text.txt";
        file.Size = 1000;
        file.LastModified = DateTime.Now;
        file.Hash = "abc123";

        cache.Add(file);

        Console.WriteLine("Файл є: " + cache.Contains("text.txt"));

        FileMetadata? found = cache.Get("text.txt");

        if (found != null)
        {
            Console.WriteLine("Назва: " + found.FileName);
            Console.WriteLine("Розмір: " + found.Size);
            Console.WriteLine("Дата: " + found.LastModified);
            Console.WriteLine("Хеш: " + found.Hash);
        }

        cache.Remove("text.txt");
        Console.WriteLine("Після видалення: " + cache.Contains("text.txt"));

        Console.WriteLine("\n=== Теги статей ===");

        TagManager manager = new TagManager();

        manager.AddTag("1", "CSharp");
        manager.AddTag("1", ".NET");
        manager.AddTag("1", "CSharp");

        manager.AddTag("2", ".NET");
        manager.AddTag("2", "Web");

        Console.WriteLine("Усі теги: " +
            string.Join(", ", manager.GetAllUniqueTags()));

        Console.WriteLine("Спільні теги: " +
            string.Join(", ", manager.FindCommonTags("1", "2")));

        Console.WriteLine("Тільки в першій статті: " +
            string.Join(", ", manager.FindDifferentTags("1", "2")));

        manager.RemoveTag("2", "Web");

        Console.WriteLine("Друга стаття після видалення тега: " +
            string.Join(", ", manager.GetTags("2")));
    }
}