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

	public class QuartzNode: QuartzPluginBase {

        #region ICodeGenerator Members

        public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) 
        {
			base.Init( project, dependencies, aggregator );
		}

        public override void Generate()
        {
            foreach (Process process in ProcessList) {
                string dir = GetProcessDir(process);
                Directory.CreateDirectory(dir);
                foreach (INode raw in process.GetBusinessNodeList()) {
                    Node node = raw as Node;
                    if (node == null) {
                        continue;
                    }
                    string output = Path.Combine(dir, node.Name + ".cs");
                    Templates.Generate(GetResource("Node.cs.vtl"), output, GetVariables(process, node));
                    Log.Info("Generated '{0}'", output);
                }
            }
        }

        private Dictionary<string, object> GetVariables(Process process, Node node)
        {
            Dictionary<string, object> vars = new Dictionary<string, object>();

            vars.Add("process", process);
            vars.Add("node", node);
            vars.Add("base", GetParam("BaseClass","BusinessNode"));
            vars.Add("namespace", GetParam("Namespace", "Frontend"));
            vars.Add("switchBased", GetParam("SwitchBased", "false"));
            vars.Add("author", Project.Parameters["Author"]);

            return vars;
        }

		public override string Name {
			get { return "Quartz.Node"; }
		}

		#endregion ICodeGenerator Members

	};

}

