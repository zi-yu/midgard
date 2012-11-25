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

namespace Odin.Plugin {

    public class Template : PluginBase, ICodeGenerator {

        #region ICodeGenerator Members

        public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
        }

        public void Generate() {
            try {
                string templateFile = Path.Combine( Globals.TemplateDirectory, GetParam("TemplateFile") );
                string outputFile = Path.Combine(Project.OutputPath, GetParam("OutputFile"));

                Templates.Generate(templateFile, outputFile, GetGeneralParameters());

                Log.Info("Generated file `{1}' from template `{0}", templateFile, outputFile);
            } catch(LokiException ex) {
                Log.Error(ex.Message);
            }
        }

        public void BeforeGenerate() {
        }

        public void AfterGenerate() {
        }

        public override Dictionary<string, string> DefaultParameters {
            get {
                Dictionary<string, string> p = new Dictionary<string, string>();

                p.Add("TemplateFile", string.Empty);
                p.Add("OutputFile", string.Empty);
                p.Add("PropertiesFile?", string.Empty);

                return p;
            }
        }

        /// <summary>
        /// Plugin Parameters
        /// </summary>
        public override void SetParameters( Dictionary<string, string> parameters ) {
            pluginParameters = parameters;
        }

        #endregion

    };

}