using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;

namespace Midgard.Interop
{
	/// <summary>
	/// Utility class to obtain information pertinent to CgoConfig.
	/// </summary>
	public sealed class CgoConfigs
	{
		/// <summary>
		/// Searches for all CgoConfig given the criteria. It will search for
		/// services with a similar code or name.
		/// </summary>
		/// <returns>List of matching services.</returns>
		public static CgoConfig[] Search()
		{
			string xpath = string.Format( CultureInfo.InvariantCulture, 
				@"field" );

			XmlDocument doc = LoadServiceList();
			XmlNodeList nodeList = doc.DocumentElement.SelectNodes(xpath);

			CgoConfig[] results = new CgoConfig[ nodeList.Count ];

			for ( int i=0; i<results.Length; i++ )
			{
                results[i] = new CgoConfig(nodeList[i].Attributes["code"].Value, 
                    nodeList[i].Attributes["name"].Value,
                    nodeList[i].Attributes["id"] == null ? null : nodeList[i].Attributes["id"].Value,
                    nodeList[i].Attributes["label"] == null ? null : nodeList[i].Attributes["label"].Value,
                    nodeList[i].Attributes["summary"] == null ? null : nodeList[i].Attributes["summary"].Value,
                    nodeList[i].Attributes["type"] == null ? null : nodeList[i].Attributes["type"].Value,
                    nodeList[i].Attributes["size"] == null ? null : nodeList[i].Attributes["size"].Value,
                    nodeList[i].Attributes["lovType"] == null ? null : nodeList[i].Attributes["lovType"].Value);
			}

			return results;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cgo"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public static bool Exists(CgoConfig[] cgo,string code)
		{
			bool found=false;
			for(int i=0;i<cgo.Length;i++)
			{
				if(cgo[i].Code==code)
				{
					found=true;
                    i=cgo.Length;
				}
			}
			return found;
		}

		/// <summary>
		/// Loads all of the CgoConfig from file.
		/// </summary>
		/// <returns>Document with all of the CgoConfigs.</returns>
		public static XmlDocument LoadServiceList()
		{
			FileInfo info = new FileInfo( Assembly.GetExecutingAssembly().Location );
			string filename = Path.Combine( info.DirectoryName, "Cgo-Config.xml" );

            XmlTextReader reader = new XmlTextReader(filename);
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ValidationType = ValidationType.Schema;
            XmlReader objXmlReader = XmlReader.Create(reader, readerSettings);

            XmlDocument doc = new XmlDocument();

            // TODO: Add validation
            doc.Load(objXmlReader);

			return doc;
		}
	}
}
