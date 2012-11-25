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
using System.Collections.Generic;
using System.Text;
using Loki.Interfaces;
using Loki.Generic;
using System.IO;
using Loki.Exceptions;
using Loki.DataRepresentation;

namespace Odin.Plugin {

	public class GenerateConfigApp : PluginBase, ICodeGenerator {

		#region IPlugin Members

		public override string Name {
			get { return "Odin.GenerateConfigApp"; }
		}

		public override void SetParameters( Dictionary<string, string> parameters ) {
			pluginParameters = parameters;
		}
		
		public override Dictionary<string, string> DefaultParameters {
			get { return new Dictionary<string, string>(); }
		}

		#endregion

		#region ICodeGenerator Members

		public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
			Aggregator.RegisterComponent( ComponentType.Config.ToString() );
            Aggregator.RegisterGacAssembly( ComponentType.Config.ToString(), "System.Xml" );
			Aggregator.RegisterAssembly( ComponentType.Config.ToString(), "Mono.GetOptions.dll" );
			Aggregator.RegisterAssembly( ComponentType.Config.ToString(), "Loki.dll" );
            Aggregator.RegisterAssembly( ComponentType.Config.ToString(), "NVelocity.dll" );
            Aggregator.RegisterGacAssembly(ComponentType.Config.ToString(), "System.Web");
			Aggregator.RegisterComponentReference( ComponentType.Config.ToString(), ComponentType.DataAccessLayer.ToString() );
			Aggregator.RegisterComponentReference( ComponentType.Config.ToString(), ComponentType.Core.ToString() );
			Aggregator.RegisterProjectType( ComponentType.Config.ToString(), ProjectTypes.Exe );
		}

		public void Generate() {

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "project", Project );
			variables.Add( "namespace", Project.Name + "." + ComponentType.Config.ToString() );
			variables.Add( "className", Project.Name );
            variables.Add( "dependencies", Dependencies );
            variables.Add("localizationNamespace", Project.Name +"." + ComponentType.WebUserInterface.ToString());
			
			string output = Path.Combine( Project.OutputPath, "Config" );
			Directory.CreateDirectory( output );

			output = Path.Combine( output, string.Format( "{0}Config.cs", Project.Name ) );
			Aggregator.RegisterFile( ComponentType.Config.ToString(), output );
			Templates.Generate( GetResource( "GenerateConfigApp.vtl" ), output, variables );
		}

		public void BeforeGenerate() {
			
		}

		public void AfterGenerate() {
            string output = Path.Combine(Project.OutputPath, ComponentType.WebUserInterface.ToString());
            output = Path.Combine(output, "bin/Template.properties.txt");
            File.Copy(GetResource("Template.properties.txt"), output);
		}

		#endregion
	}
}
