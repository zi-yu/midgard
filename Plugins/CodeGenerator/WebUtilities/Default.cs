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

	public class Default : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );
		}

		public override void Generate() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["namespace"] = Namespace;
			variables["projectName"] = Project.Name;
			variables["controlsNamespace"] = ControlsNamespace;

			GenerateDefaultCode( variables );
			GenerateDefaultControl( variables );
			GenerateErrorDefaultControl( variables );
			GenerateMaintenanceControl( variables );
		}

		public override string Name {
			get { return "Web.Default"; }
		}

		#endregion

		#region Generate

		private void GenerateDefaultCode( Dictionary<string, object> variables ) {
			string output = GetRootOutputDir( "default.aspx.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "default.aspx" );

			Templates.Generate( GetResource( "Default.aspx.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateDefaultControl( Dictionary<string, object> variables ) {
			string output = GetRootOutputDir( "default.aspx" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "Default.aspx.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateErrorDefaultControl( Dictionary<string, object> variables ) {
			string output = GetRootOutputDir( "error.aspx" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "Error.aspx.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateMaintenanceControl( Dictionary<string, object> variables ) {
			string output = GetRootOutputDir( "maintenance.html" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "Maintenance.aspx.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion
		
	};

}
