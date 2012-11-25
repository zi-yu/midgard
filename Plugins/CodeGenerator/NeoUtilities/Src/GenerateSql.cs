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

    public class GenerateSql : PluginBase, ICodeGenerator {

        #region ICodeGenerator Members

        public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
        }

        public void Generate() {
            Log.Info("Sql scripts generated!");
        }

        public void BeforeGenerate() {
        }

        public void AfterGenerate() {
        }

		public override string Name {
			get	{ return "Neo.GenerateSql"; }
		}

        public override Dictionary<string, string> DefaultParameters {
            get {
                Dictionary<string, string> p = new Dictionary<string, string>();

                p.Add("SourcePath", string.Empty);
                p.Add("DestinyPath", string.Empty);

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

    };

}