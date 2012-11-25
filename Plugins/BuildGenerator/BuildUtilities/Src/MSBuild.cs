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

namespace Odin.Plugin {

	public class MSBuild : PluginBase, IBuildGenerator {

		#region Private Fields

		private Dictionary<string, BuildInformation> information = new Dictionary<string, BuildInformation>();

		#endregion

        #region Properties

        protected Dictionary<string, BuildInformation> Information {
            get { return information; }
        }

        #endregion

		#region Constructor

		public MSBuild() {}

		#endregion

		#region IPlugin Members

		public override string Name {
			get { return "Odin.MSBuild"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get {
				return new Dictionary<string, string>();
			}
		}

		#endregion

		#region IBuildGenerator Members

		public virtual void Init( IProject project, IDependencyManager dependencies, Dictionary<string, BuildInformation> info ) {
			Project = project;
            information = info;
            Dependencies = dependencies;
		}

		public void Generate() {
			if( information.Count == 0 ) {
				Log.Info( "There is no information to construct a build file!" );
			} else {
				GenerateProjects();
				GenerateSolution();
			}
		}

		private void GenerateSolution() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "buildInformation", information.Values );
			variables.Add( "projectGuid", Guid.NewGuid().ToString().ToUpper() );
			variables.Add( "webGuid", Guid.NewGuid().ToString().ToUpper() );

			string output = Path.Combine( Project.OutputPath, Project.Name + ".sln" );
			Templates.Generate( GetResource( "SolutionTemplate.vtl" ), output, variables );
		}

		private void GenerateProjects() {
			Dictionary<string, BuildInformation>.Enumerator iter = information.GetEnumerator();

			Log.Info( "Starting Generating MSBuild Files..." );
			while( iter.MoveNext() ) {
				Log.Info( string.Format( "Generating MSBuild File for {0}", iter.Current.Key ) );

				Dictionary<string, object> variables = new Dictionary<string, object>();
				variables.Add( "projectName", iter.Current.Key );
				variables.Add( "information", iter.Current.Value );
				variables.Add( "extensionName", Project.Name );
				variables.Add( "projectType", iter.Current.Value.ProjectType.ToString() );
				variables.Add( "webProjectName", ComponentType.WebUserInterface.ToString() );

				string output = Path.Combine( Project.OutputPath, iter.Current.Key );
				output = Path.Combine( output, string.Format( "{0}.csproj", iter.Current.Key ) );
				Templates.Generate( GetResource( "MSBuildTemplate.vtl" ), output, variables );

				Log.Info( "Build file {0} generated!", iter.Current.Key );
			}

			Log.Info( "Build Files Generated!" );
		}

		#endregion
	};

}