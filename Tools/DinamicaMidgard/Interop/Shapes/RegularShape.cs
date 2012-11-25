using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Office.Interop.Visio;
using Midgard.XmlForms;

namespace Midgard.Interop.Shapes
{
	/// <summary>
	/// Base class for shape handlers.
	/// </summary>
	public abstract class RegularShape : BaseShape
	{
		/// <summary>
		/// Initializes a new instance of the NodeShape class.
		/// </summary>
		public RegularShape( object shape, XmlForm form, XmlElement attributes ) : base( shape, form, attributes )
		{
		}


		/// <summary>
		/// Performs context validation on the current shape.
		/// </summary>
		/// <param name="validationData">Context dependent validation data.</param>
		/// <returns>A non-empty string in case of error, otherwise null.</returns>
		protected override string DoContextValidation( object validationData )
		{
			StringBuilder sb = new StringBuilder();

			object[] o = (object[]) validationData;
			ArrayList from = (ArrayList) o[ 0 ];
			ArrayList to   = (ArrayList) o[ 1 ];


			if ( to == null || to.Count == 0 )
				sb.Append( Resources.GetString( ResourceTokens.NodeShapeNotConnected ) );

			return sb.ToString();
		}
	}
}
