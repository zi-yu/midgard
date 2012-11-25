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

	public class Monodevelop : PluginBase, IBuildGenerator {

		#region Private Fields

		private Dictionary<string, BuildInformation> information = new Dictionary<string, BuildInformation>();

		#endregion

        #region Properties

        protected Dictionary<string, BuildInformation> Information {
            get { return information; }
        }

        #endregion

		#region Constructor

		public Monodevelop() {}

		#endregion

		#region IPlugin Members

		public override string Name {
			get { return "Odin.Monodevelop"; }
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
		
		public string ToProjectComponent( object obj )
		{
			BuildFile file = (BuildFile) obj;
			string path = file.Name;
			path = path.Replace( Project.OutputPath, string.Empty );
			path = path.Substring( file.Component.Length + 1 );
			
			return path;
			
		}

		private void GenerateSolution() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "buildInformation", information.Values );
			variables.Add( "name", Project.Name );

			string output = Path.Combine( Project.OutputPath, Project.Name + ".mds" );
			Templates.Generate( GetResource( "Mds.vtl" ), output, variables );
		}

		private void GenerateProjects() {
			Dictionary<string, BuildInformation>.Enumerator iter = information.GetEnumerator();

			Log.Info( "Starting Generating Monodevelop Project Files..." );
			while( iter.MoveNext() ) {
				Log.Info( string.Format( "Generating Monodevelop Project File for {0}", iter.Current.Key ) );

				Dictionary<string, object> variables = new Dictionary<string, object>();
				variables.Add( "projectName", iter.Current.Key );
				variables.Add( "information", iter.Current.Value );
				variables.Add( "extensionName", Project.Name );
				variables.Add( "relative", this );
				variables.Add( "projectType", iter.Current.Value.ProjectType.ToString() );
				variables.Add( "webProjectName", ComponentType.WebUserInterface.ToString() );

				string output = Path.Combine( Project.OutputPath, iter.Current.Key );
				output = Path.Combine( output, string.Format( "{0}.mdp", iter.Current.Key ) );
				Templates.Generate( GetResource( "Mdp.vtl" ), output, variables );

				Log.Info( "Build file {0} generated!", iter.Current.Key );
			}

			Log.Info( "Build Files Generated!" );
		}

		#endregion
	};

}