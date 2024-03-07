using LuceneSearchDotnet;
using System.Text.Json;
using System.Text.Json.Serialization;
using newtonsoft =Newtonsoft.Json;
var personFile = File.ReadAllText("PersonData.json");
var persons = JsonSerializer.Deserialize<List<Person>>(personFile, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive =true});

var engine = new PersonSearchEngine();
if (persons == null) return;

engine.AddPersonsToIndex(persons);


while (true)
{
    Console.Clear();
    Console.WriteLine("Enter a Search query:");
    var query = Console.ReadLine();
    if(string.IsNullOrEmpty(query)) return;

    var results = engine.Search(query);
    if(results == null || results.Count() == 0)
    {
        Console.WriteLine("No Results found");
        continue;
    }

    Console.WriteLine("Results");
    foreach(var person in results)
    {
        Console.WriteLine(newtonsoft.JsonConvert.SerializeObject(person,newtonsoft.Formatting.Indented));
    }
    Console.WriteLine("Press any key to continue");
    Console.ReadKey();
}