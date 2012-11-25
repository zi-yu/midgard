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

using Loki.Generic;
using Loki.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Odin.Plugin {

	public class Nant : PluginBase, IBuildGenerator {

		#region Private Fields

		private Dictionary<string, BuildInformation> information = new Dictionary<string, BuildInformation>();

		#endregion

        #region Properties

        protected Dictionary<string, BuildInformation> Information {
            get { return information; }
        }

        #endregion

		#region Constructor

		public Nant() {}

		#endregion

		#region IPlugin Members

		public override string Name {
			get { return "Odin.Nant"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get {
				return new Dictionary<string, string>();
			}
		}

		#endregion

		#region IBuildGenerator Members

		public void Init( IProject project, IDependencyManager dependencies, Dictionary<string, BuildInformation> info ) {
			Project = project;
			information = info;
            Dependencies = dependencies;
		}

		public void Generate() {
			if( information.Count == 0 ) {
				Log.Info( "There is no information to construct a build file!" );
			} else {
				Log.Info( "Starting Generating Nant Build Files..." );

				Dictionary<string, object> variables = new Dictionary<string, object>();

				variables.Add( "projectName", Project.Name );
                variables.Add("dependencies", Dependencies);
				variables.Add( "pluginName", Name );
				variables.Add( "extensionName", Project.Name );
				variables.Add( "webProjectName", ComponentType.WebUserInterface.ToString() );
				variables.Add( "dir", System.Environment.CurrentDirectory );
                variables.Add("configApp", Project.Name + "." + ComponentType.Config.ToString() + ".exe" );

				GatheringAllInfo(variables);

				string output = Path.Combine( Project.OutputPath, Project.Name + ".build" );
				Templates.Generate( GetResource( "NantTemplate.vtl" ), output, variables );

				Log.Info( "Build Files Generated!" );
			}
		}

		private void GatheringAllInfo( Dictionary<string, object> variables ) {
			Dictionary<string, BuildInformation>.Enumerator iter = information.GetEnumerator();

			List<BuildInformation> informationList = new List<BuildInformation>();

			string s = string.Empty;
			while( iter.MoveNext() ) {
				informationList.Add( iter.Current.Value );
				s += string.Format( "build-{0},", iter.Current.Value.Name );
			}

			s = s.Substring( 0, s.Length - 1 );

			variables.Add( "information", informationList );
			variables.Add( "allProjects", s );
		}
		
		#endregion
	};
}