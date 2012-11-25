#region Licence Statment
// Copyright (c) Zi-Yu.com - All Rights Reserved
// http://midgard.zi-yu.com/
//
// The use and distribution terms for this software are covered by the
// LGPL (http://opensource.org/licenses/lgpl-license.php).
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using NVelocity.App;
using NVelocity;
using Loki.Generic;
using NVelocity.Exception;
using Commons.Collections;

namespace Loki.Generic {
    public class Templates {

        #region NVelocity Static Members

        public static void Generate( string templateFile, string outputFile, Dictionary<string, object> param ) {
            Generate(null, templateFile, outputFile, param);
        }

        public static void Generate(string propertiesFile, string templateFile, string outputFile, Dictionary<string, object> param)
        {
            Generate(null, templateFile, outputFile, false, param);
        }

        public static void Generate( string propertiesFile, string templateFile, string outputFile, bool overwrite, Dictionary<string, object> param ) {
            if( File.Exists(outputFile) && !overwrite ){
                Log.Warn("*** [NVelocity] File '{0}' already exists, bailing out", outputFile);
                return;
            }

            if( !string.IsNullOrEmpty(propertiesFile) ) {
                Velocity.Init(propertiesFile);
            } else {
                Velocity.Init("Template.properties.txt");
            }

	        VelocityContext context = GetContextFromDictionary(param);
    	    Template template = null;

			string file = templateFile;

            try {
                template = Velocity.GetTemplate(file);
                using ( TextWriter stream = GetOutputStream(outputFile) ) {
                    template.Merge(context, stream);
                }
	        } catch (ResourceNotFoundException ex) {
		        Log.Error("Template error : cannot find template '{0}'", file);
				Log.Error(ex);
	        } catch (ParseErrorException pee) {
                Log.Error("Template error : Syntax error in template '{0}'", templateFile + ":" + pee);
				Log.Error(pee);
	        }
	    }

        private static TextWriter GetOutputStream(string outputFile)
        {
            if (outputFile == null) {
                return new StringWriter();
            }
            return new StreamWriter(outputFile);
        }

        private static VelocityContext GetContextFromDictionary(Dictionary<string,object> param)
        {
            VelocityContext context = new VelocityContext();
            foreach( KeyValuePair<string, object> pair in param ) {
                context.Put(pair.Key, pair.Value);
            }
            return context;
        }

        #endregion

		#region XSLT Static Members

		public static void Transform(string xmlPath, string xsltPath, string outputFile)
		{
			XPathDocument xpath = new XPathDocument(xmlPath);
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(xsltPath);

			XmlTextWriter writer = new XmlTextWriter(outputFile, null);

			string result = Transform(xpath, xslt);

			writer.WriteRaw(result);
			writer.Close();
		}

		public static string Transform(string rawXml, string xsltFile)
		{
			StringReader reader = new StringReader(rawXml);
			XPathDocument source = new XPathDocument(reader);

			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(xsltFile);

			return Transform(source, xslt);
		}

		public static string Transform(IXPathNavigable source, string xsltFile)
		{
			XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(xsltFile);

			return Transform(source, xslt);
		}

		public static string Transform(IXPathNavigable source, XslCompiledTransform xslt)
		{
			XsltArgumentList args = new XsltArgumentList();

			StringWriter writer = new StringWriter();
			xslt.Transform(source, args, writer);

			return writer.ToString();
		}

		#endregion

	};
}
