
using System.Text.Json;
using Lucene;

// See https://aka.ms/new-console-template for more information
var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

var entities = File.ReadAllText("PersonSample.json");
var people = JsonSerializer.Deserialize<IEnumerable<Person>>(entities, options);

var engine = new PersonSearchEngine();

if(people==null) return;

engine.AddPeopleToIndex(people);

while(true)
{
    Console.Clear();
    Console.WriteLine("Loaded {0} people into the index.", people.Count());
    Console.WriteLine($"Total bytes used: {engine.GetTotalBytes()}");
    Console.WriteLine("Check out the Lucene query syntax here: ");
    Console.WriteLine("https://lucene.apache.org/core/2_9_4/queryparsersyntax.html");
    Console.WriteLine("----------------------------------------------------------");
    Console.WriteLine("Enter a search term: ");    
    var searchTerm = Console.ReadLine();

    if(string.IsNullOrWhiteSpace(searchTerm)) continue;

    var results = engine.Search(searchTerm);

    Console.WriteLine();
    Console.WriteLine("Results:");
    foreach(var result in results)
    {
        Console.WriteLine($"({result.Company}) - {result.Name} - {result.Email} - {result.Phone}");
    }

    Console.WriteLine();
    Console.WriteLine("Press ESC to exit, any other key to search again.");
    
    var key = Console.ReadKey();
    if(key.Key == ConsoleKey.Escape) break;        
}


