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
using Loki.DataRepresentation.Loaders;
using Quartz;

namespace Odin.Plugin {

	public class QuartzGlobal : QuartzPluginBase {

		#region Private

		private bool HasTransiction(List<Transiction> transictions, Transiction transiction) {
			return transictions.FindAll(delegate(Transiction t){ return t.EventName == transiction.EventName; }).Count != 0;
			/*
			foreach (Transiction t in transictions) {
				if (t.EventName == transiction.EventName) {
					return true;
				}
			}
			return false;*/
		}

		#endregion

		#region Protected

		protected string GetParam(string key, string defaultValue) {
			if (pluginParameters == null || !pluginParameters.ContainsKey(key)) {
				return defaultValue;
			}

			return pluginParameters[key];
		}

		protected Dictionary<string, object> GetVariables() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["namespace"] = GetParam("Namespace", "Quartz");
			variables["transictions"] = GetTransictions();

			return variables;
		}

		protected List<Transiction> GetTransictions() {
			List<Transiction> transictions = new List<Transiction>();

			List<Process> processes = (List<Process>)Project.Generic["Quartz"];
			foreach (Process process in processes) {
				foreach (INode node in process.NodeList.Values) {
					foreach (Transiction transiction in node.Transictions) {
						if(!HasTransiction(transictions, transiction)) {
							transictions.Add(transiction);
						}
					}
				}
			}
			return transictions;
		}

		#endregion

        #region ICodeGenerator Members

        public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
            base.Init(project, dependencies, aggregator);
		}

		public override void Generate() {
			string output = GetRelativeOutputDir("Global.cs", GenericFolder);
			Templates.Generate(GetResource("Global.cs.vtl"), output, GetVariables());
			Log.Info( "Generated '{0}'", output );
		}

		public override string Name {
			get { return "Quartz.Global"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get {
				Dictionary<string, string> p = new Dictionary<string, string>();
				p.Add("Namespace?", string.Empty);
				return p;
			}
		}

		#endregion ICodeGenerator Members

	};

}

