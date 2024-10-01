using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace Lucene;

public record Person
{
    public Guid Guid { get; init; }
    public string? Name { get; init; }
    public string? Company { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
}

public class PersonSearchEngine
{
    private static readonly LuceneVersion VERSION = LuceneVersion.LUCENE_48;
    private readonly StandardAnalyzer _analyser;
    private readonly RAMDirectory _directory;
    private readonly IndexWriter _writer;


    public PersonSearchEngine()
    {
        _analyser = new(VERSION);
        _directory = new();
        
        _writer = new(_directory, new IndexWriterConfig(VERSION, _analyser));
    }

    public string GetTotalBytes()
    {
        long bytes = _directory.GetSizeInBytes();
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1048576) return $"{bytes / 1024:F2} KB";
        if (bytes < 1073741824) return $"{bytes / 1048576:F2} MB";
        if (bytes < 1099511627776) return $"{bytes / 1073741824:F2} GB";
        return $"{bytes / 1099511627776:F2} TB";
    }

    public void AddPeopleToIndex(IEnumerable<Person> entities)
    {
        foreach (var item in entities)
        {
            var document = new Document
            {
                new StringField(nameof(Person.Guid), item.Guid.ToString(), Field.Store.YES),
                new TextField(nameof(Person.Name), item.Name, Field.Store.YES),
                new TextField(nameof(Person.Company), item.Company, Field.Store.YES),
                new TextField(nameof(Person.Email), item.Email, Field.Store.YES),
                new TextField(nameof(Person.Phone), item.Phone, Field.Store.YES)
            };

            _writer.AddDocument(document);
        }

        _writer.Commit();
    }

    public IEnumerable<Person> Search(string searchTerm)
    {
        using var directoryReader = DirectoryReader.Open(_directory);

        var indexSearcher = new IndexSearcher(directoryReader);

        string[] fields = [nameof(Person.Name), nameof(Person.Company), nameof(Person.Email), nameof(Person.Phone)];

        var queryParser = new MultiFieldQueryParser(VERSION, fields, _analyser);

        var query = queryParser.Parse(searchTerm);

        var hints = indexSearcher.Search(query, 10).ScoreDocs;

        foreach (var item in hints)
        {
            var doc = indexSearcher.Doc(item.Doc);
            yield return new Person
            {
                Guid = Guid.Parse(doc.Get(nameof(Person.Guid))),
                Name = doc.Get(nameof(Person.Name)),
                Company = doc.Get(nameof(Person.Company)),
                Email = doc.Get(nameof(Person.Email)),
                Phone = doc.Get(nameof(Person.Phone))
            };
        }
    }
}