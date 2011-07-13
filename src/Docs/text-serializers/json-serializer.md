# JSON Serializer

ServiceStack's JsonSerializer is optimized for serializing C# POCO types into and from JSON as fast, compact and cleanly as possible. In most cases C# objects serializes as you would expect them to without added json extensions or serializer-specific artefacts.

JSON is a lightweight text serialization format with a spec that's so simple that it fits on one page: [http://www.json.org](json.org).

The only valid values in JSON are:

  * string
  * number
  * object
  * array
  * true
  * false
  * null

Where most allowed values are scalar and the only complex types available are objects and arrays. Although limited, the above set of types make a good fit and can express most programming data structures.

### number, true, false types

All boolean and numeric data types are stored as-is without quotes.

### null type

For the most compact output null values are omitted from the serialized by default. If you want to include null values set:

	JsConfig.IncludeNullValues = true;

### string type

All other scalar values are stored as strings that are surrounded with double quotes.

### C# Structs and Value Types

Because a C# struct is a value type whose public properties are normally just convenience properties around a single scalar value, they are ignored instead the **TStruct.ToString()** method is used to serialize and either the **static TStruct.Parse()** method or **new TStruct(string)** constructor will be used to deserialize the value type if it exists.

### array type

Any List, Queue, Stack, Array, Collection, Enumerables including custom enumerable types are stored in exactly the same way as a JavaScript array literal, i.e:

	[1,2,3,4,5]

All elements in an array must be of the same type. If a custom type is both an IEnumerable and has properties it will be treated as an array and the extra properties will be ignored.

### object type

The JSON object type is the most flexible and is how most complex .NET types are serialized. The JSON object type is a key-value pair JavaScript object literal where the key is always a double-quoted string.

Any IDictionary is serialized into a standard JSON object, i.e:

	{"A":1,"B":2,"C":3,"D":4}

Which happens to be the same as C# POCO types (inc. Interfaces) with the values:

`new MyClass { A=1, B=2, C=3, D=4 }`

	{"A":1,"B":2,"C":3,"D":4}

Only public properties on reference types are serialized with the C# Property Name used for object key and the Property Value as the value. At the moment it is not possible to customize the Property Name.

JsonSerializer also supports serialization of anonymous types in much the same way:

`new { A=1, B=2, C=3, D=4 }`

	{"A":1,"B":2,"C":3,"D":4}

## Custom Deserialization

Because the same wire format shared between Dictionaries, POCOs and anonymous types, in most cases what you serialize with one type can be serialized with another, i.e. an Anonymous type can be deserialized back into a Dictionary<string,string> which can be read into a strong-typed POCO and vice-versa.

Although the JSON Serializer is best optimized for serializing and deserializing .NET types, it's flexible enough to consume 3rd party JSON apis although this generally requires custom de-serialization to convert it into an idiomatic .NET type.

[https://github.com/ServiceStack/ServiceStack.Text/blob/master/tests/ServiceStack.Text.Tests/UseCases/GitHubRestTests.cs](GitHubRestTests.c)

  1. Using [JsonObject](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsonObject.cs)
  2. Using Generic .NET Collection classes
  3. Using Customized DTO's in the shape of the 3rd party JSON response

The [CentroidTests](https://github.com/ServiceStack/ServiceStack.Text/blob/master/tests/ServiceStack.Text.Tests/UseCases/CentroidTests.cs) is another example that uses the JsonObject to parse a complex custom JSON response. 


