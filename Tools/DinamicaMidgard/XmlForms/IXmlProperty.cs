using System;
using System.Xml;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Defines an XML property.
	/// </summary>
	public interface IXmlProperty
	{
		/// <summary>
		/// Gets the ID for this property. Property ID are unique across
		/// a shape.
		/// </summary>
		string Id
		{
			get;
		}

		/// <summary>
		/// Gets the label of the current property.
		/// </summary>
		string Label
		{
			get;
		}

		/// <summary>
		/// Gets wether the currently property is valid or not.
		/// </summary>
		bool IsValid
		{
			get;
		}

		/// <summary>
		/// Fills the XML form data based on the serialization data.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		void Set( XmlElement element );

		/// <summary>
		/// Fills the destination element with the serialization data for the
		/// property.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		/// <returns>True if the element is to be append to the shape XML, false otherwise.</returns>
		bool Fill( XmlElement element );
	}
}
