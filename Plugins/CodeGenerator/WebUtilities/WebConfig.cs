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

	public class WebConfig : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );

			
		}

		public override void Generate() {
			string output = GetRootOutputDir( "Web.config" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.None );

			Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
			variables.Add( "namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() );
            variables.Add("prefix", Project.Name );
            variables.Add("controlsNamespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() + ".Controls");
            variables.Add("assembly", Project.Name + "." + ComponentType.WebUserInterface.ToString());
            

			Templates.Generate( GetResource( "WebConfig.vtl" ), output, variables );
		}

		public override string Name {
			get { return "Web.WebConfig"; }
		}

		#endregion
	};

}
