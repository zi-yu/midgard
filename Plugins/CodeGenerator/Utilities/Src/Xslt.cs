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
using Loki.Generic;
using Loki.Exceptions;
using Loki.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace Odin.Plugin {

	public class Xslt : PluginBase, ICodeGenerator {

        #region ICodeGenerator Members

        public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
        }

        public void Generate() {
			try {
				string xmlFile = GetParam("XmlFile");
				string xsltFile = GetParam("XsltFile");
				string outputFile = GetParam("OutputFile");
				outputFile = Path.Combine(Project.OutputPath, outputFile);

				Templates.Transform(xmlFile, xsltFile, outputFile);

				Log.Info("XML file `{0}' transformed to `{1}' using `{2}'",
							xmlFile,
							outputFile,
							xsltFile
					);

			} catch (LokiException ex) {
				Log.Error(ex);
			} catch (Exception ex) {
				Log.Error("Error applying xslt `{0}' to file `{1}', output to `{2}'!",
							GetParam("XsltFile"),
							GetParam("XmlFile"),
							GetParam("OutputFile")
					);
				Log.Error(ex);
			}
        }
		
        public void BeforeGenerate() {
        }

		public void AfterGenerate()	{
		}

        public override Dictionary<string, string> DefaultParameters {
            get {
                Dictionary<string, string> p = new Dictionary<string, string>();

                p.Add("XsltFile", string.Empty);
                p.Add("XmlFile", string.Empty);
				p.Add("OutputFile", string.Empty);

                return p;
            }
        }

        #endregion

    };

}