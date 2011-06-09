# JSV Text Format (JSON + CSV)

Type Serializer uses a hybrid CSV-style escaping + JavaScript-like text-based format that is optimized for both size and speed. I'm naming this JSV-format (i.e. JSON + CSV) 

In many ways it is similar to JavaScript, e.g. any List, Array, Collection of ints, longs, etc are stored in exactly the same way, i.e:
	[1,2,3,4,5]

Any IDictionary is serialized like JavaScript, i.e:
	{A:1,B:2,C:3,D:4}

Which also happens to be the same as C# POCO class with the values 

`new MyClass { A=1, B=2, C=3, D=4 }`

	{A:1,B:2,C:3,D:4}

JSV is *white-space significant*, which means normal string values can be serialized without quotes, e.g: 

`new MyClass { Foo="Bar", Greet="Hello World!"}` is serialized as:

	{Foo:Bar,Greet:Hello World!}


### CSV escaping

Any string with any of the following characters: `[]{},"`
is escaped using CSV-style escaping where the value is wrapped in double quotes, e.g:

`new MyClass { Name = "Me, Junior" }` is serialized as:
	
	{Name:"Me, Junior"}

A value with a double-quote is escaped with another double quote e.g:

`new MyClass { Size = "2\" x 1\"" }` is serialized as:

	{Size:"2"" x 1"""}
