using Xunit;
using System.Collections.Generic;
using System.Linq;

public class CsvProcessorTests
{
    [Fact]
    public void CalculateNameFrequencies_ShouldReturnCorrectFrequencies()
    {
        // Arrange
        var processor = new CsvProcessor("fake.csv");

        var people = new List<Person>
    {
        new("Matt", "Brown", "12 Acton St"),
        new("Heinrich", "Jones", "31 Clifton Rd"),
        new("Johnson", "Smith", "22 Jones Rd"),
        new("Tim", "Johnson", "99 Abbey Rd")
    };

        // Act
        var frequencies = processor.CalculateNameFrequencies(people)
            .Select(line => line.Split(','))
            .Select(parts => (Name: parts[0].Trim(), Count: int.Parse(parts[1].Trim())))
            .ToList();

        // Assert
        Assert.Collection(frequencies,
            item => Assert.Equal(("Johnson", 2), item),
            item => Assert.Equal(("Brown", 1), item),
            item => Assert.Equal(("Heinrich", 1), item),
            item => Assert.Equal(("Jones", 1), item),
            item => Assert.Equal(("Matt", 1), item),
            item => Assert.Equal(("Smith", 1), item),
            item => Assert.Equal(("Tim", 1), item)
        );
    }


    [Fact]
    public void SortAddressesByStreetName_ShouldReturnAddressesSortedByStreet()
    {
        // Arrange
        var processor = new CsvProcessor("fake.csv");

        var people = new List<Person>
        {
            new("FirstName", "LastName", "31 Clifton Rd"),
            new("FirstName", "LastName", "12 Acton St"),
            new("FirstName", "LastName", "22 Jones Rd")
        };

        // Act
        var sortedAddresses = processor.SortAddressesByStreetName(people);

        // Assert
        var expected = new List<string> { "12 Acton St", "31 Clifton Rd", "22 Jones Rd" };
        Assert.Equal(expected, sortedAddresses);
    }
}
