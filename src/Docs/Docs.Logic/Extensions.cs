using System.Text.RegularExpressions;
using ServiceStack;

namespace Docs.Logic
{
	public static class Extensions
	{
		static readonly Regex AlphanumericRegex = new Regex("[^a-zA-Z-]", RegexOptions.Compiled);

		public static string SafeName(this string name)
		{
			return AlphanumericRegex.Replace(name.Replace(" ", "-").ToLower(), "");
		}

		public static T DeepClone<T>(this T src)
		{
			return src.ToJsv().FromJsv<T>();
		}
	}
}