using System.Text;

public class CsvProcessor
{
    private readonly string _inputFilePath;

    public CsvProcessor(string inputFilePath)
    {
        _inputFilePath = inputFilePath ?? throw new ArgumentNullException(nameof(inputFilePath));
    }

    public List<Person> ReadCsv()
    {
        if (!File.Exists(_inputFilePath))
            throw new FileNotFoundException($"Input file not found: {_inputFilePath}");

        return File.ReadLines(_inputFilePath)
            .Select(line => line.Split(','))
            .Where(parts => parts.Length >= 3)
            .Select(parts => new Person(
                parts[0].Trim(),
                parts[1].Trim(),
                parts[2].Trim()
            ))
            .ToList();
    }

    public IEnumerable<string> CalculateNameFrequencies(List<Person> people)
    {
        if (people is null) throw new ArgumentNullException(nameof(people));

        var nameCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var person in people)
        {
            IncrementNameCount(nameCounts, person.FirstName);
            IncrementNameCount(nameCounts, person.LastName);
        }

        return nameCounts
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
            .Select(kv => $"{kv.Key}, {kv.Value}");
    }

    public List<string> SortAddressesByStreetName(List<Person> people)
    {
        if (people is null) throw new ArgumentNullException(nameof(people));

        return people
            .Where(p => !string.IsNullOrWhiteSpace(p.Address))
            .Select(p => p.Address)
            .OrderBy(address => GetStreetName(address), StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static void IncrementNameCount(Dictionary<string, int> counts, string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;

        if (counts.TryGetValue(name, out var count))
            counts[name] = count + 1;
        else
            counts[name] = 1;
    }

    private static string GetStreetName(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return string.Empty;

        var parts = address.Trim().Split(' ', 2);
        return parts.Length > 1 ? parts[1] : address;
    }
}
