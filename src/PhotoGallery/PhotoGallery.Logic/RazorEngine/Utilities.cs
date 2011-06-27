#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          © West Wind Technologies, 2010-2011
 *          http://www.west-wind.com/
 * 
 * Created: 1/4/2010
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace RazorEngine
{
	/// <summary>
	/// Helper class that provides a few simple utilitity functions to the project
	/// </summary>
	public class Utilities
	{
		/// <summary>
		/// Returns a relative path based on a base path.
		/// 
		/// Examples:
		/// &lt;&lt;ul&gt;&gt;
		/// &lt;&lt;li&gt;&gt; filename.txt
		/// &lt;&lt;li&gt;&gt; subDir\filename.txt
		/// &lt;&lt;li&gt;&gt; ..\filename.txt
		/// &lt;&lt;li&gt;&gt; ..\..\filename.txt
		/// &lt;&lt;/ul&gt;&gt;
		/// <seealso>Class Utilities</seealso>
		/// </summary>
		/// <param name="fullPath">
		/// The full path from which to generate a relative path
		/// </param>
		/// <param name="basePath">
		/// The base path based on which the relative path is based on
		/// </param>
		/// <returns>string</returns>
		public static string GetRelativePath(string fullPath, string basePath)
		{
			// ForceBasePath to a path
			if (!basePath.EndsWith("\\"))
				basePath += "\\";

			Uri baseUri = new Uri(basePath);
			Uri fullUri = new Uri(fullPath);

			Uri relativeUri = baseUri.MakeRelativeUri(fullUri);

			// Uri's use forward slashes so convert back to backward slahes
			return relativeUri.ToString().Replace("/", "\\");
		}

		/// <summary>
		/// HTML-encodes a string and returns the encoded string.
		/// </summary>
		/// <param name="text">The text string to encode. </param>
		/// <returns>The HTML-encoded text.</returns>
		public static string HtmlEncode(string text)
		{
			if (text == null)
				return string.Empty;

			StringBuilder sb = new StringBuilder(text.Length);

			int len = text.Length;
			for (int i = 0; i < len; i++)
			{
				switch (text[i])
				{

					case '<':
						sb.Append("&lt;");
						break;
					case '>':
						sb.Append("&gt;");
						break;
					case '"':
						sb.Append("&quot;");
						break;
					case '&':
						sb.Append("&amp;");
						break;
					default:
						if (text[i] > 159)
						{
							// decimal numeric entity
							sb.Append("&#");
							sb.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
							sb.Append(";");
						}
						else
							sb.Append(text[i]);
						break;
				}
			}
			return sb.ToString();
		}
	}
}
