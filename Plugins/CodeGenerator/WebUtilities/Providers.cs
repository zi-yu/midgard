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

	public class Providers : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );

			Aggregator.RegisterGacAssembly( ComponentType.WebUserInterface.ToString(), "System.Configuration" );

            WebUtilities.Dependencies.Instance.RegistDependency("Providers");
		}

		public override void Generate() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() );
			variables.Add( "roles", Project.Name + "." + ComponentType.Core + ".Roles" );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			GenerateUserProvider( variables );
		}

		public override string Name {
			get { return "Web.Providers"; }
		}

		#endregion

		#region Generate

		private void GenerateUserProvider( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "MidgardUserProvider.cs", "Providers" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "MidgardUserProvider.vtl" ), output, variables );
		}

		private void GenerateRoleProvider( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "MidgardRoleProvider.cs", "Providers" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "MidgardRoleProvider.vtl" ), output, variables );
		}

		#endregion
	};

}
