
using System;
using System.IO;
namespace RazorEngine
{

	/// <summary>
	/// Base class used for Razor Page Templates - Razor generates
	/// a class from the parsed Razor markup and this class is the
	/// base class. Class must implement an Execute() method that 
	/// is overridden by the parser and contains the code that generates
	/// the markup.  Write() and WriteLiteral() must be implemented
	/// to handle output generation inside of the Execute() generated
	/// code.
	/// 
	/// This class can be subclassed to provide custom functionality.
	/// One common feature likely will be to provide Context style properties
	/// that are application specific (ie. HelpBuilderContext) and strongly
	/// typed and easily accesible in Razor markup code.   
	/// </summary>
	public class RazorTemplateBase<TModel> : RazorTemplateBase
		where TModel : class
	{
		/// <summary>
		/// Create a strongly typed model
		/// </summary>
		public TModel Model { get; set; }
	}

	/// <summary>
	/// Base class used for Razor Page Templates - Razor generates
	/// a class from the parsed Razor markup and this class is the
	/// base class. Class must implement an Execute() method that 
	/// is overridden by the parser and contains the code that generates
	/// the markup.  Write() and WriteLiteral() must be implemented
	/// to handle output generation inside of the Execute() generated
	/// code.
	/// 
	/// This class can be subclassed to provide custom functionality.
	/// One common feature likely will be to provide Context style properties
	/// that are application specific (ie. HelpBuilderContext) and strongly
	/// typed and easily accesible in Razor markup code.   
	/// </summary>
	public class RazorTemplateBase : MarshalByRefObject, IDisposable
	{
		/// <summary>
		/// You can pass in a generic context object
		/// to use in your template code
		/// </summary>
		public dynamic Context { get; set; }

		/// <summary>
		/// Any optional result data that the template
		/// might have to create and return to the caller
		/// </summary>
		public dynamic ResultData { get; set; }


		/// <summary>
		/// Class that generates output. Currently ultra simple
		/// with only Response.Write() implementation.
		/// </summary>
		public RazorResponse Response { get; set; }


		/// <summary>
		/// Class that provides request specific information.
		/// May or may not have its member data set.
		/// </summary>
		public RazorRequest Request { get; set; }


		/// <summary>
		/// Instance of the HostContainer that is hosting
		/// this Engine instance. Note that this may be null
		/// if no HostContainer is used.
		/// 
		/// Note this object needs to be cast to the 
		/// the appropriate Host Container
		/// </summary>
		public object HostContainer { get; set; }

		/// <summary>
		/// Instance of the RazorEngine object.
		/// </summary>
		public object Engine { get; set; }


		/// <summary>
		/// This method is called upon instantiation
		/// and allows passing custom configuration
		/// data to the template from the Engine.
		/// 
		/// This method can then be overridden        
		/// </summary>
		/// <param name="configurationData"></param>
		public virtual void InitializeTemplate(object context, object configurationData)
		{

		}


		public RazorTemplateBase()
		{
			Response = new RazorResponse();
			Request = new RazorRequest();
		}


		public virtual void Write(object value)
		{
			// For HTML output we'd probably want to HTMLEncode everything
			//Response.Write(Utilities.HtmlEncode(value));

			// But not for plain text templating
			Response.Write(value);
		}

		public virtual void WriteString(object value)
		{
			Response.Write(value.ToString() + "\r\n");
		}

		public virtual void WriteLiteral(object value)
		{
			Response.Write(value);
		}

		public virtual string HtmlEncode(string input)
		{
			return Utilities.HtmlEncode(input);
		}

		/// <summary>
		/// Razor Parser overrides this method
		/// </summary>
		public virtual void Execute() { }


		public virtual void Dispose()
		{
			if (Response != null)
			{
				Response.Dispose();
				Response = null;
			}
		}
	}
}
