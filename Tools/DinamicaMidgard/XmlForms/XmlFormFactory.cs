using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using Midgard.XmlForms.Configuration;


namespace Midgard.XmlForms
{
	/// <summary>
	/// Provides method to create properties.
	/// </summary>
	public sealed class XmlFormFactory
	{
		/// <remarks>
		/// Prevent class initialization.
		/// </remarks>
		private XmlFormFactory()
		{
		}


		/// <summary>
		/// Creates a control from the property definition.
		/// </summary>
		/// <param name="element">Element in the XML definition file.</param>
		/// <param name="data">Data to pass to the control.</param>
		/// <returns>Instance of Control.</returns>
		public static Control CreateProperty( XmlElement element, object data, Microsoft.Office.Interop.Visio.Page page)
		{
			string prefix = FormsNamespace.NamespaceManager.LookupPrefix( element.NamespaceURI );
			string localName = element.LocalName;

            string type;

		    if (prefix != null && prefix != "") 
		        type = string.Format("{0}:{1}", prefix, localName);
		    else 
		        type = localName;

		    Property property = null;

            foreach (Property p in FormConfigs.Properties)
			{
				if ( p.Type == type )
				{
					property = p;
					break;
				}
			}


			if ( property == null )
				throw new Exception( "No registered property handler. (" + element.Name + ")" );


			Control control;

			try
			{
                if(property.Type.Contains("itemList"))
				    control = (Control) TypeFactory.Create( property.Moniker, new object[] { element, data,  page } );
                else
                    control = (Control)TypeFactory.Create(property.Moniker, new object[] { element, data});
			}
			catch ( InvalidCastException ex )
			{
				throw new Exception( "Error creating property handler.", ex );
			}

			return control;
		}


	}
}
