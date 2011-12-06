# ServiceStack JsonSerializer

Benchmarks for .NET's JSON Serializers are available at: [servicestack.net/benchmarks/](http://www.servicestack.net/benchmarks/)

ServiceStack's JsonSerializer is optimized for serializing C# POCO types in and out of JSON as fast, compact and cleanly as possible. In most cases C# objects serializes as you would expect them to without added json extensions or serializer-specific artefacts.

JsonSerializer provides a simple API that allows you to serialize any .NET generic or runtime type into a string, TextWriter/TextReader or Stream.

### Serialization API

	string SerializeToString<T>(T)
	void SerializeToWriter<T>(T, TextWriter)
	void SerializeToStream<T>(T, Stream)
	string SerializeToString(object, Type)
	void SerializeToWriter(object, Type, TextWriter)
	void SerializeToStream(object, Type, Stream)

### Deserialization API

	T DeserializeFromString<T>(string)
	T DeserializeFromReader<T>(TextReader)
	object DeserializeFromString(string, Type)
	object DeserializeFromReader(reader, Type)
	object DeserializeFromStream(Type, Stream)
	T DeserializeFromStream<T>(Stream)

### Extension methods

	string ToJson<T>(this T)
	T FromJson<T>(this string)

Convenient **ToJson/FromJson** extension methods are also included reducing the amount of code required, e.g:

	new []{ 1, 2, 3 }.ToJson()   //= [1,2,3]
	"[1,2,3]".FromJson<int[]>()  //= int []{ 1, 2, 3 }

## JSON Format 

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

All C# boolean and numeric data types are stored as-is without quotes.

### null type

For the most compact output null values are omitted from the serialized by default. If you want to include null values set the global configuration:

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


## Custom Serialization

Although JsonSerializer is optimized for serializing .NET POCO types, it still provides some options to change the convention-based serialization routine.

### Using Structs to Customize JSON

This makes it possible to customize the serialization routine and provide an even more compact wire format. 

E.g. Instead of using a JSON object to represent a point 

	{ Width=20, Height=10 }
	
You could use a struct and reduce it to just: 

	"20x10" 

By overriding **ToString()** and providing a static **Size Parse()** method:

	public struct Size
	{
		public double Width { get; set; }
		public double Height { get; set; }

		public override string ToString()
		{
			return Width + "x" + Height;
		}

		public static Size Parse(string json)
		{
			var size = json.Split('x');
			return new Size { 
				Width = double.Parse(size[0]), 
				Height = double.Parse(size[1]) 
			};
		}
	}

Which would change it to the more compact JSON output:

	new Size { Width = 20, Height = 10 }.ToJson() // = "20x10"

That allows you to deserialize it back in the same way:

	var size = "20x10".FromJson<Size>(); 

### Using Custom IEnumerable class to serialize a JSON array

In addition to using a Struct you can optionally use a custom C# IEnumerable type to provide a strong-typed wrapper around a JSON array:

	public class Point : IEnumerable
	{
		double[] points = new double[2];
	
		public double X 
		{
			get { return points[0]; }
			set { points[0] = value; }
		}
	
		public double Y
		{
			get { return points[1]; }
			set { points[1] = value; }
		}
	
		public IEnumerator GetEnumerator()
		{
			foreach (var point in points) 
				yield return point;
		}
	}

Which serializes the Point into a compact JSON array:

	new Point { X = 1, Y = 2 }.ToJson() // = [1,2]

## Custom Deserialization

Because the same wire format shared between Dictionaries, POCOs and anonymous types, in most cases what you serialize with one type can be deserialized with another, i.e. an Anonymous type can be deserialized back into a Dictionary<string,string> which can be deserialized into a strong-typed POCO and vice-versa.

Although the JSON Serializer is best optimized for serializing and deserializing .NET types, it's flexible enough to consume 3rd party JSON apis although this generally requires custom de-serialization to convert it into an idiomatic .NET type.

[GitHubRestTests.cs](https://github.com/ServiceStack/ServiceStack.Text/blob/master/tests/ServiceStack.Text.Tests/UseCases/GitHubRestTests.cs)

  1. Using [JsonObject](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsonObject.cs)
  2. Using Generic .NET Collection classes
  3. Using Customized DTO's in the shape of the 3rd party JSON response

[CentroidTests](https://github.com/ServiceStack/ServiceStack.Text/blob/master/tests/ServiceStack.Text.Tests/UseCases/CentroidTests.cs) is another example that uses the JsonObject to parse a complex custom JSON response. 


