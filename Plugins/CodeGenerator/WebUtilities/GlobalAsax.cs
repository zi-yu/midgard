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

	public class GlobalAsax : WebPluginBase {

		#region Fields

		private delegate void Dependency();
	
		#endregion

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );

		}

		public override void Generate() {
			GenerateAsaxCode();
			GenerateAsaxControl();
		}

		public override string Name {
			get { return "Web.GlobalAsax"; }
		}

		#endregion

		#region Generate
		
		private void GenerateAsaxCode() {
			string output = GetRootOutputDir( "Global.asax.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, false, "Global.asax" );

			Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
			variables.Add( "namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			Templates.Generate( GetResource( "Global.asax.cs.vtl" ), output, variables );
		}

		private void GenerateAsaxControl() {
			string output = GetRootOutputDir( "Global.asax" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() );

			Templates.Generate( GetResource( "Global.asax.vtl" ), output, variables );
		}

		#endregion


	};

}
