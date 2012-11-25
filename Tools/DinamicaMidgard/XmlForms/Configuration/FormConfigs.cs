using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.Globalization;

namespace Midgard.XmlForms.Configuration
{
    /// <summary>
    /// Contains the configuration settings for an XML Forms application.
    /// </summary>
    public sealed class FormConfigs
    {
        /// <summary>
        /// Gets the extension used for shape files.
        /// </summary>
        public const string ShapeExtension = "shape";

        #region Private Members

        private static Namespace[] _namespaces;
        private static Property[] _properties;
        private static DirectoryInfo _directory;

        #endregion


        /// <summary>
        /// Static initialization of configuration settings.
        /// </summary>
        static FormConfigs()
        {
            FileInfo assembly = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string configFile = AssemblyConfiguration.GetConfigurationFile(assembly.FullName);
            //string configFile = assembly.FullName + ".config";
            XmlReader reader = new XmlTextReader(configFile);

            // TODO: Use XmlValidatingReader instead :/
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);


            #region Property Configuration

            ArrayList properties = new ArrayList();

            foreach (XmlElement el in doc.SelectNodes("/configuration/properties/property"))
            {
                string type = el.Attributes["type"].Value;
                string moniker = el.Attributes["moniker"].Value;
                Property p = new Property(type, moniker);

                properties.Add(p);
            }

            _properties = (Property[])properties.ToArray(typeof(Property));

            #endregion

            #region Namespaces Configuration

            ArrayList namespaces = new ArrayList();

            foreach (XmlElement el in doc.SelectNodes("/configuration/namespaces/namespace"))
            {
                string prefix = el.Attributes["prefix"].Value;
                string uri = el.Attributes["uri"].Value;
                Namespace n = new Namespace(prefix, uri);

                namespaces.Add(n);
            }

            _namespaces = (Namespace[])namespaces.ToArray(typeof(Namespace));

            #endregion

            #region Shape Library: Location
            
            XmlAttribute locationAttr = (XmlAttribute)doc.SelectSingleNode("/configuration/shapeLibrary/location/@dir");

            if (locationAttr.Value.StartsWith("~"))
            {
                string locationDir = string.Format("{0}{1}.{2}",
                    assembly.Directory.FullName,
                    Path.DirectorySeparatorChar,
                    locationAttr.Value.Substring(1));

                _directory = new DirectoryInfo(locationDir);
            }
            else
            {
                _directory = new DirectoryInfo(locationAttr.Value);
            }
             
            //_directory = new DirectoryInfo(Path.Combine(assembly.DirectoryName, "Shapes"));

            #endregion
        }



        /// <summary>
        /// Gets the absolute filename for the given shape.
        /// </summary>
        /// <param name="prefix">Prefix of the filename.</param>
        /// <param name="shapeName">Name of shape.</param>
        /// <returns>Absolute file path where file should be.</returns>
        /// <remarks>
        /// Please note that this method does not guarantee that the file exists!
        /// You must perform error management when using using this value.
        /// </remarks>
        public static string GetShapeFile(string shapeName)
        {
            string filename = string.Format(CultureInfo.InvariantCulture,
                "{0}.{1}", shapeName, FormConfigs.ShapeExtension);

            return Path.Combine(_directory.FullName, filename);
        }



        /// <summary>
        /// Gets the list of namespaces.
        /// </summary>
        public static Namespace[] Namespaces
        {
            get { return _namespaces; }
        }


        /// <summary>
        /// Gets the list of properties.
        /// </summary>
        public static Property[] Properties
        {
            get { return _properties; }
        }
    }
}