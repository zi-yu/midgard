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

	public class ExceptionLogger : WebPluginBase {

		#region Properties

		public override string Name {
			get { return "Web.ExceptionLogger"; }
		}

		#endregion

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );
			
			Aggregator.RegisterComponentReference( ComponentType.WebUserInterface.ToString(), ComponentType.DataAccessLayer.ToString() );
			Aggregator.RegisterComponentReference( ComponentType.WebUserInterface.ToString(), ComponentType.Core.ToString() );
			Aggregator.RegisterGacAssembly( ComponentType.WebUserInterface.ToString(), "System.Web" );
			Aggregator.RegisterAssembly( ComponentType.WebUserInterface.ToString(), "Loki.dll" );

            WebUtilities.Dependencies.Instance.RegistDependency("ExceptionLogger");
		}

		public override void Generate() {
			GenerateExceptionLogger();
			GenerateExceptionModule();
		}

		#endregion

		#region Private

		private void GenerateExceptionLogger() {
			string output = GetRelativeOutputDir( "ExceptionLogger.cs", "Engine" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "dataAccess", Project.Name + "." + ComponentType.DataAccessLayer.ToString() );
			variables.Add( "core", Project.Name + "." + ComponentType.Core.ToString() );
			variables.Add( "namespace", Namespace );

			Templates.Generate( GetResource( "ExceptionLogger.vtl" ), output, variables );
		}

		private void GenerateExceptionModule() {
			string output = GetRelativeOutputDir( "ExceptionModule.cs", "Modules" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );
			variables.Add( "modulesNamespace", ModulesNamespace );

			Templates.Generate( GetResource( "ExceptionModule.cs.vtl" ), output, variables );
		}

		#endregion

	};

}
