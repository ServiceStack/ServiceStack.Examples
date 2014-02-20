#ServiceStack's CSV Format

The [CSV format](http://en.wikipedia.org/wiki/Comma-separated_values) is now a first-class supported format which means all your existing web services can automatically take advantage of the new format without any config or code changes. Just drop the latest ServiceStack.dlls (v1.77+) and you're good to go! 

### Importance of CSV
CSV is an important format for transferring, migrating and quickly visualizing data as all spreadsheets support viewing and editing CSV files directly whilst its supported by most RDBMS support exporting and importing data. Compared with other serialization formats, it provides a compact and efficient way to transfer large datasets in an easy to read text format.

### Speed
The CSV Serializer used was developed using the same tech that makes [ServiceStack's JSV and JSON serializers fast](http://www.servicestack.net/benchmarks/NorthwindDatabaseRowsSerialization.100000-times.2010-08-17.html) (i.e. no run-time reflection, static delegate caching, etc), which should make it the fastest CSV serializer available for .NET.

### Downloadable Separately
The CsvSerializer is maintained in the [ServiceStack.Text](~/text-serializers/json-csv-jsv-serializers) project and is available as a [separate download](https://github.com/ServiceStack/ServiceStack.Text/downloads) for use in your own .NET projects.

### How to register your own custom format with ServiceStack
What makes the 'CSV' format different is its the first format added using the new extensions API. The complete code to register the CSV format is:

    //Register the 'text/csv' content-type and serializers (format is inferred from the last part of the content-type)
    this.ContentTypeFilters.Register(ContentType.Csv,
        CsvSerializer.SerializeToStream, CsvSerializer.DeserializeFromStream);

    //Add a response filter to add a 'Content-Disposition' header so browsers treat it natively as a .csv file
    this.ResponseFilters.Add((req, res, dto) =>
        {
            if (req.ResponseContentType == ContentType.Csv)
            {
                res.AddHeader(HttpHeaders.ContentDisposition,
                    string.Format("attachment;filename={0}.csv", req.OperationName));
            }
        });


Note: **ServiceStack already does this for you** though it still serves a good example to show how you can plug-in your own custom format. If you wish, you can remove all custom formats with (inside AppHost.Configure()):
    this.ContentTypeFilters.ClearCustomFilters();
    
The ability to automatically to register another format and provide immediate value and added functionality to all your existing web services (without any code-changes or configuration) we believe is a testament to ServiceStack's clean design of using strongly-typed 'message-based' DTOs to let you develop clean, testable and re-usable web services. No code-gen or marshalling is required to bind to an abstract method signature, every request and calling convention maps naturally to your web services DTOs.


## Usage
The CSV format is effectively a first-class supported format so everything should work as expected, including being registered as an available format on ServiceStack's metadata index page:

* [/servicestack/metadata](http://www.servicestack.net/ServiceStack.MovieRest/servicestack/metadata)

And being able to preview the output of a service:

* [/servicestack/csv/metadata?op=Movie](http://www.servicestack.net/ServiceStack.MovieRest/servicestack/csv/metadata?op=Movie)

By default they are automatically available using ServiceStack's standard calling conventions, e.g:

* [/csv/syncreply/Movies](http://www.servicestack.net/ServiceStack.MovieRest/csv/syncreply/Movies)
    
### REST Usage
CSV also works just as you would expect with user-defined RESTful urls, i.e. you can append ?format=csv to specify the format in the url e.g:

* [/movies?format=csv](http://www.servicestack.net/ServiceStack.MovieRest/movies?format=csv)

This is how the above web service output looks when opened up in [google docs](https://spreadsheets.google.com/pub?key=0AjnFdBrbn8_fdDBwX0Rha04wSTNWZDZlQXctcmp2bVE&hl=en_GB&output=html)


Alternative in following with the HTTP specification you can also specify content-type `"text/csv"` in the *Accept* header of your HttpClient, e.g:

    var httpReq = (HttpWebRequest)WebRequest.Create("http://mono.servicestack.net/ServiceStack.MovieRest/movies");
    httpReq.Accept = "text/csv";
    var csv = new StreamReader(httpReq.GetResponse().GetResponseStream()).ReadToEnd();


## Limitations
As most readers familiar with the CSV format will know there are some inherent limitations with CSV-format namely it is a flat-structured tabular data format that really only supports serialization of a single result set. 
This limitation remains, although as the choice of what to serialize is based on the following conventions: 

* If you only return one result in your DTO it will serialize that.
* If you return multiple results it will pick the first IEnumerable<> property or if it doesn't exist picks the first property.
* Non-enumerable results are treated like a single row.

Basically if you only return 1 result it should work as expected otherwise it will chose the best candidate based on the rules above.

The second major limitation is that it doesn't yet include a CSV deserializer (currently on the TODO list), so while you can view the results in CSV format you can't post data to your web service in CSV and have it automatically deserialize for you. You can however still upload a CSV file and parse it manually yourself.

#Features
Unlike most CSV serializers that can only serialize rows of primitive values, the CsvSeriliaizer uses the [JSV Format](~/text-serializers/jsv-format) under the hood so even [complex types](https://spreadsheets.google.com/pub?key=0AjnFdBrbn8_fdG83eWdGM1lnVW9PMlplcmVDYWtXeVE&hl=en_GB&output=html) will be serialized in fields in a easy to read format - no matter how deep its hierarchy.


Feel free to discuss or find more about any of these features at the [Service Stack Google Group](https://groups.google.com/forum/#!forum/servicestack)

[<Wiki Home](~/framework/home)
