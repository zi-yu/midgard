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
using Loki.Interfaces;
using System.IO;
using System.Collections.Generic;
using Loki.Exceptions;
using Loki.DataRepresentation;
using WebUtilities;

namespace Odin.Plugin {

    public class Xml2aspx : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) 
        {
			base.Init( project, dependencies, aggregator );
            dependencies.RegistDependency("Xml2aspx");
		}

		public override void Generate() 
        {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["namespace"] = Namespace;
			variables["controlsNamespace"] = ControlsNamespace;
			variables["modulesNamespace"] = ModulesNamespace;
            variables["entities"] = Project.Model;
            variables["prefix"] = "$prefix";
            variables["project"] = Project.Name;

            GenerateTemplate(variables, "Xml2aspx.NamedTemplates.xslt");
            GenerateTemplate(variables, "Xml2aspx.Entities.xslt");
            GenerateTemplate(variables, "Xml2aspx.Screen.xslt");
            GenerateTemplate(variables, "Xml2aspx.Main.xslt");
		}

		public override string Name {
            get { return "Web.Xml2aspx"; }
		}

        public override string Description{
            get {
                return "Parses XML UI format to ASPX";
            }
        }

        public override Dictionary<string, string> DefaultParameters {
            get {
                Dictionary<string, string> args = new Dictionary<string, string>();
                return args;
            }
        }

		#endregion

		#region Generate

		private void GenerateTemplate( Dictionary<string, object> variables, string template ) 
        {
            string output = Path.Combine(OutputDir, template);
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.None );

			Templates.Generate( GetResource( template ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion

        #region Utilities

        public string OutputDir {
            get {
                string output = Path.Combine(GetOutputDir(), "Xslt");
                if (!Directory.Exists(output)) {
                    Directory.CreateDirectory(output);
                }

                output = Path.Combine(output, "Xml2aspx");
                if (!Directory.Exists(output)) {
                    Directory.CreateDirectory(output);
                }

                return output;
            }
        }

        #endregion Utilities

    };

}
