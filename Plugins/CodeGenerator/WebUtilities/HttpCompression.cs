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
using Loki.DataRepresentation.Loaders;

namespace Odin.Plugin {

    public class HttpCompression : WebPluginBase {

        #region ICodeGenerator Members

		public override void Generate() 
        {
            GenerateChannelControl();
		}

        private void GenerateChannelControl()
        {
            string output = GetRelativeOutputDir("HttpCompression.cs", "Modules");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = BuildArgs();
            Templates.Generate(GetResource("CompressionModule.cs.vtl"), output, variables);
        }

        private Dictionary<string, object> BuildArgs()
        {
            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
            variables.Add("namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() + ".Modules");
            variables.Add("coreMamespace", Project.Name + "." + ComponentType.Core.ToString());
            variables.Add("dalMamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("prefix", Project.Name );
            return variables;
        }

		public override string Name {
			get { return "Web.HttpCompression"; }
		}

		#endregion

	};

}

