using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Utility class to obtain information pertinent to ComboBoxObject.
	/// </summary>
	public sealed class ComboBoxUtils
	{
		/// <summary>
		/// Searches for all ComboBoxObject given the criteria. It will search in config file for
		/// services with a similar code or name.
		/// </summary>
		/// <returns>List of matching services.</returns>
		public static ComboBoxObject[] Search(string nodeType, string fileName)
		{
			string xpath = string.Format( CultureInfo.InvariantCulture,
                string.Format("{0}",nodeType));

			ComboBoxObject[] results=null;

            XmlDocument doc = LoadServiceListFromFile(fileName);
			if(doc!=null)
			{
				XmlNodeList nodeList = doc.DocumentElement.SelectNodes( xpath );
				results = new ComboBoxObject[ nodeList.Count ];

				//results[0]=new ComboBoxObject(string.Empty,string.Empty);

				for ( int i=0; i<results.Length; i++ )
				{
					string code = nodeList[ i ].Attributes[ "code" ].Value;
					string name = nodeList[ i ].Attributes[ "name" ].Value;

					results[ i ] = new ComboBoxObject( code, name);
				}
			}

			return results;
		}


		/// <summary>
		/// Loads all of the ComboBoxObject from file.
		/// </summary>
		/// <returns>Document with all of the configs.</returns>
		private static XmlDocument LoadServiceListFromFile(string file)
		{
			FileInfo info = new FileInfo( Assembly.GetExecutingAssembly().Location );
            string filename = Path.Combine(info.DirectoryName, file);
			XmlDocument doc = null;

			try
			{
				XmlTextReader reader = new XmlTextReader( filename );
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.ValidationType = ValidationType.Schema;
                XmlReader objXmlReader = XmlReader.Create(reader, readerSettings);

                doc=new XmlDocument( );

				// TODO: Add validation
				doc.Load( objXmlReader );
			} catch(Exception)
			{
				doc=null;
			}

			return doc;
		}
	}
}
