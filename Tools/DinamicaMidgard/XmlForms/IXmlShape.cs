using System;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Interface that has to be implemented by shape handlers **with respect
	/// to the XML forms engine**.
	/// </summary>
	public interface IXmlShape
	{
		/// <summary>
		/// Adds shape-specific wiring to the XML-based WinForm controls.
		/// </summary>
		void Design();

        /// <summary>
        /// Changes shape-specific properties
        /// </summary>
        void ChangeShapeProperties(string prop, string value);

        string ConnectedTo();
        string ConnectedFrom();
    }
}
