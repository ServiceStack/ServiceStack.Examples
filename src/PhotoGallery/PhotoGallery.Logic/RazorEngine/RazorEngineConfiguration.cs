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
using System.IO;

namespace RazorEngine
{
	/// <summary>
	/// Configuration for the Host class. These settings determine some of the
	/// operational parameters of the RazorHost class that can be changed at
	/// runtime.
	/// </summary>        
	public class RazorEngineConfiguration : MarshalByRefObject
	{
		/// <summary>
		/// Determines if assemblies are compiled to disk or to memory.
		/// If compiling to disk generated assemblies are not cleaned up
		/// </summary>
		public bool CompileToMemory
		{
			get { return _CompileToMemory; }
			set { _CompileToMemory = value; }
		}
		private bool _CompileToMemory = true;

		/// <summary>
		/// When compiling to disk use this Path to hold generated assemblies
		/// </summary>
		public string TempAssemblyPath
		{
			get
			{
				if (!string.IsNullOrEmpty(_TempAssemblyPath))
					return _TempAssemblyPath;

				return Path.GetTempPath();
			}
			set { _TempAssemblyPath = value; }
		}
		private string _TempAssemblyPath = null;

		/// <summary>
		/// Encoding to be used when generating output to file
		/// </summary>
		public Encoding OutputEncoding
		{
			get { return _OutputEncoding; }
			set { _OutputEncoding = value; }
		}
		private Encoding _OutputEncoding = Encoding.UTF8;


		/// <summary>
		/// Buffer size for streamed template output when using filenames
		/// </summary>
		public int StreamBufferSize
		{
			get { return _StreamBufferSize; }
			set { _StreamBufferSize = value; }
		}
		private int _StreamBufferSize = 2048;

	}
}