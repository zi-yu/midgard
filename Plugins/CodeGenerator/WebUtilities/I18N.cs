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

	public class I18N : WebPluginBase {

		#region Properties

		
		#endregion

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );
            WebUtilities.Dependencies.Instance.RegistDependency("I18N");
		}

		public override void Generate() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["namespace"] = Namespace;
			variables["controlsNamespace"] = ControlsNamespace;
			variables["modulesNamespace"] = ModulesNamespace;

			GenerateResourceFile();
			GenerateResourceLabel( variables );
			GenerateLanguageModule( variables );
			GenerateLanguageManager( variables );
		}

		public override string Name {
			get { return "Web.I18N"; }
		}

		#endregion

		#region Generate

		private void GenerateResourceFile() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["namespace"] = Namespace;
			variables["controlsNamespace"] = ControlsNamespace;
			variables["modulesNamespace"] = ModulesNamespace;

			foreach( string s in ResourceRegister.Resources.Keys ) {
				variables["className"] = s;
				variables["resources"] = ResourceRegister.Resources[s];
				GenerateResourceDefault( s, variables );
				GenerateResourceHandler( s, variables );
			}
		}

		private void GenerateResourceDefault( string s, Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir(string.Format("{0}.resx",s),"App_GlobalResource" );
			Aggregator.RegisterResource( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "Resource.resx.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateResourceHandler( string s, Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( string.Format("{0}.Designer.cs",s), "App_GlobalResource" );
			Aggregator.RegisterResource( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "Resource.Designer.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateResourceLabel( Dictionary<string, object> variables ) {
			string output = GetControlsOutputDir( "Generic", "Label.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "Label.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLanguageModule( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "LanguageModule.cs", "Modules" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "LanguageModule.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLanguageManager( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "LanguageManager.cs", "Engine" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "LanguageManager.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion
		
	};

}
