using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using Microsoft.Office.Interop.Visio;

namespace Midgard.Interop
{
	/// <summary>
	/// Describes methods on a Midgard shape.
	/// </summary>
	public interface IShapeHandler
	{
		/// <summary>
		/// Gets the Midgard Model shape type.
		/// </summary>
		ModelShapeType Type
		{
			get;
		}

		/// <summary>
		/// Gets the name of the serialization element.
		/// </summary>
		string Element
		{
			get;
		}

		/// <summary>
		/// Gets a reference to the underlying Visio shape.
		/// </summary>
		Shape VisioShape
		{
			get;
		}

		/// <summary>
		/// Requests that the shape process the shape command.
		/// </summary>
		/// <param name="command">Command requested by the shape.</param>
		/// <param name="context">Execution context: these are the arguments specified in the event. Please
		/// note that the command is one of the arguments.</param>
		/// <param name="settings">Display settings.</param>
		/// <returns>New display settings.</returns>
		DisplaySettings Execute( string command, NameValueCollection context, DisplaySettings settings, bool forceChangeWithoutShow );

		/// <summary>
		/// Loads the properties from the Visio shape.
		/// </summary>
		void LoadProperties();

		/// <summary>
		/// Returns the XML properties for the current shape.
		/// </summary>
		/// <returns>Xml node with properties.</returns>
		XmlNode GetProperties();

		/// <summary>
		/// Validates a shape.
		/// </summary>
		/// <returns>An instance of ShapeError if errors were found, otherwise returns null.</returns>
		ShapeError Validate( object validationData );

		/// <summary>
		/// 
		/// </summary>
		bool PaintAsInvalid();

		/// <summary>
		/// 
		/// </summary>
		bool PaintAsValid();

        bool PaintAsNoMatch();

		/// <summary>
		/// Gets the text being displayed on the shape.
		/// </summary>
		string Text
		{
			get;
		}
	}
}
