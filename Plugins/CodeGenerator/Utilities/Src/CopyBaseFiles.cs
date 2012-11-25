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

namespace Odin.Plugin {

	public class CopyBaseFiles : PluginBase, ICodeGenerator {

        #region ICodeGenerator Members

        public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
        }

        public void Generate() {
        }

        public void BeforeGenerate() {
        }

        public void AfterGenerate() {
            try {
                Dictionary<string, object> variables = new Dictionary<string, object>();
                variables.Add("copy", this);

                Templates.Generate(GetResource("CopyAssemblies.vtl"), null, variables);

            }catch( Exception e ) {
                Log.Error(e);
            }
        }

        public override Dictionary<string, string> DefaultParameters {
            get {
                Dictionary<string, string> p = new Dictionary<string, string>();
                return p;
            }
        }

        /// <summary>
        /// Plugin Parameters
        /// </summary>
        public override void SetParameters(Dictionary<string, string> parameters)
        {
            pluginParameters = parameters;
        }

        #endregion

        #region Copy

        public void file(object source, object target)
        {
            ICodeGenerator copy = new CopyFile();
            copy.Init(Project, Dependencies, Aggregator);

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("SourcePath", source.ToString());
            param.Add("DestinyPath", target.ToString());

            copy.SetParameters(param);
            copy.AfterGenerate();
        }

        #endregion Copy
    }

}