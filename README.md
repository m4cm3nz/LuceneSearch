# Lucene

A simple console POC to validate the Lucene .net basic features.
It loads some Person Json data in memory, and allow you to execute some basic query search,

## References

@hahnsoftware
* https://www.youtube.com/watch?v=WifEkt-48bM

Lucene .Net Search Engine Library
* https://lucenenet.apache.org/index.html

Lucene Query sintax
* https://lucene.apache.org/core/2_9_4/queryparsersyntax.html

## Quick Start

```C#

git clone https://github.com/m4cm3nz/LuceneSearch.git
cd LuceneSearch
dotnetrun

```

## Person JSON Sample

```C#

    {
        "guid": "6d59e5cb-ae18-4aa0-ae2f-194074d0d9e3",
        "name": "Velma Golden",
        "company": "BULLZONE",
        "email": "velmagolden@bullzone.com",
        "phone": "+1 (923) 534-2269"
    }

```