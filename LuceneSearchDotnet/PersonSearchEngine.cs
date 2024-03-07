using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearchDotnet;

public class PersonSearchEngine
{
    const LuceneVersion _version = LuceneVersion.LUCENE_48;
    private readonly StandardAnalyzer _analyzer;
    private readonly RAMDirectory _directory;
    private readonly IndexWriter _writer;

    public PersonSearchEngine()
    {
        _analyzer = new StandardAnalyzer(_version);
        _directory = new RAMDirectory();
        var config = new IndexWriterConfig(_version, _analyzer);
        _writer = new IndexWriter(_directory, config);
    }


    public void AddPersonsToIndex(IEnumerable<Person> persons)
    {
        foreach (var person in persons)
        {
            var document = new Document();
            document.Add(new StringField("Id", person.Id.ToString(), Field.Store.YES));
            document.Add(new TextField("FirstName", person.FirstName, Field.Store.YES));
            document.Add(new TextField("LastName", person.LastName, Field.Store.YES));
            document.Add(new TextField("Title", person.Title, Field.Store.YES));
            document.Add(new TextField("Email", person.Email, Field.Store.YES));
            document.Add(new TextField("Company", person.Company, Field.Store.YES));
            document.Add(new TextField("Description", person.Description, Field.Store.YES));
            _writer.AddDocument(document);
        }
        _writer.Commit();
    }

    public IEnumerable<Person> Search(string searchTerm)
    {
        var directoryReader = DirectoryReader.Open(_directory);
        var indexSearcher = new IndexSearcher(directoryReader);

        string[] fields = { "FirstName", "LastName", "Title", "Email", "Company", "Description" };
        var queryParser = new MultiFieldQueryParser(_version, fields,_analyzer);
        var query = queryParser.Parse(searchTerm);
        var hits = indexSearcher.Search(query, 10).ScoreDocs;
        var persons = new List<Person>();
        foreach(var person in hits)
        {
            var document = indexSearcher.Doc(person.Doc);
            persons.Add(new Person()
            {
                Id = new Guid(document.Get("Id")),
                FirstName = document.Get("FirstName"),
                LastName = document.Get("LastName"),
                Title = document.Get("Title"),
                Email = document.Get("Email"),
                Company = document.Get("Company"),
                Description = document.Get("Description")
            }); 
        }
        return persons;
    }
}
