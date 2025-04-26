using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: CsvProcessorApp <input-file-path>");
            return;
        }

        var inputFile = args[0];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Error: File not found: {inputFile}");
            return;
        }

        const string nameFrequencyOutput = "name_frequency.txt";
        const string addressOutput = "sorted_addresses.txt";

        try
        {
            var processor = new CsvProcessor(inputFile);
            var people = processor.ReadCsv()
                .Where(p => IsValidPerson(p))
                .ToList();

            var nameFrequencies = processor.CalculateNameFrequencies(people);
            var sortedAddresses = processor.SortAddressesByStreetName(people);

            File.WriteAllLines(nameFrequencyOutput, nameFrequencies, Encoding.UTF8);
            File.WriteAllLines(addressOutput, sortedAddresses, Encoding.UTF8);

            Console.WriteLine("✅ Processing completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ An error occurred: {ex.Message}");
        }
    }

    private static bool IsValidPerson(Person person)
    {
        if (person is null) return false;

        return !string.IsNullOrWhiteSpace(person.FirstName)
            && !string.IsNullOrWhiteSpace(person.LastName)
            && !string.IsNullOrWhiteSpace(person.Address)
            && !string.Equals(person.FirstName, "FirstName", StringComparison.OrdinalIgnoreCase);
    }
}
