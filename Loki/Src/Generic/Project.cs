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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Loki.Interfaces;
using Loki.DataRepresentation;
using Loki.DataRepresentation.Loaders;
using System.Xml;
using Loki.DataRepresentation.IntrinsicEntities;

namespace Loki.Generic {

    public class Project : IProject {

        #region Fields

        private string projectName = string.Empty;
        private Parameters parameters = new Parameters();
        private ProjectPluginParameters pluginParameters = new ProjectPluginParameters();
		private ProjectPluginParameters buildPluginParameters = new ProjectPluginParameters();
		private ProjectPluginParameters loadPluginParameters = new ProjectPluginParameters();
		private Model model = new Model();
		private Dictionary<string, object> generic = new Dictionary<string,object>();
		private string config = string.Empty;
		private delegate void LoadAux( XmlDocument doc );

        #endregion

        #region Constructor

		public Project( string projectName, string projectPath, string author, string config ) {
            this.projectName = projectName;
            AddParameter("ProjectPath", projectPath);
            AddParameter("Author", author);
			this.config = config;
			LoadConfig();
        }

        public Project( string config ) {
			this.config = config;
			LoadConfig();
        }

		#endregion

        #region Properties

        public string Name {
			get { return projectName; }
			set { projectName = value; }
        }

        public Parameters Parameters {
            get { return parameters; }
        }

        public ProjectPluginParameters PluginParameters {
            get { return pluginParameters; }
        }

		public ProjectPluginParameters BuildPluginParameters {
			get { return buildPluginParameters; }
		}

		public ProjectPluginParameters LoadPluginParameters {
			get { return loadPluginParameters; }
		}

        public string OutputPath {
            get {
				string output = null;
				output = Path.Combine(Parameters["ProjectPath"], "..");
				return new DirectoryInfo(output).FullName;
			}
			set {
				Parameters["OutputPath"] = value;
			}
        }

		public string ProjectPath {
			get { return new DirectoryInfo(Parameters["ProjectPath"]).FullName; }
			set { Parameters["ProjectPath"] = value; }
		}

		public Model Model {
			get { return model; }
			set { model = value; }
		}

		public Dictionary<string, object> Generic {
			get { return generic; }
			set { generic = value; }
		
		}

		public string Author {
			get { return Parameters["Author"]; }
			set { Parameters["Author"] = value; }
		}

		#endregion

		#region Private

		private string GetAttribute( XmlDocument doc, string attr ) {
			if( doc.DocumentElement.Attributes[attr] != null ) {
				return doc.DocumentElement.Attributes[attr].Value;
			}
			return string.Empty;
		}

		private void Load( LoadAux function ) {
			if( string.IsNullOrEmpty( config ) ) {
				Log.Error( "Configuration filename is null or empty in Project::LoadConfig " );
				return;
			}

			XmlDocument doc = new XmlDocument();
			try {
				doc.Load( config );
				function( doc );
			} catch( Exception e ) {
				Log.Error( "Error loading configuration file!" );
				Log.Error( e.ToString() );
			}
		}

		#endregion

		#region Public

		public void AddParameter( string name, string value ) {
            parameters.Add(name, value);
        }

        public void AddPluginParameter( string name, Dictionary<string,string> param ) {
            pluginParameters.Add(new KeyValuePair<string, Dictionary<string, string>>(name, param));
        }

		public void AddBuildPluginParameter( string name, Dictionary<string, string> param ) {
			buildPluginParameters.Add( new KeyValuePair<string, Dictionary<string, string>>( name, param ) );
		}

		public void AddLoadPluginParameter( string name, Dictionary<string, string> param ) {
			loadPluginParameters.Add( new KeyValuePair<string, Dictionary<string, string>>( name, param ) );
		}

		public EntityClass GetEntity( string name ) {
			foreach( Entity e in model ) {
				if( e.Name == name && e is EntityClass ) {
					return (EntityClass)e;
				}
			}
			return null;
		}

		public void LoadConfig() {
			Load( 
				delegate( XmlDocument doc ){
					Name = GetAttribute( doc, "name" );
					ProjectPath = Path.GetDirectoryName( config );
					OutputPath = GetAttribute( doc, "outputPath" );
					Author = GetAttribute( doc, "author" );

					ConfigLoader.Instance.LoadPlugins( this, doc );
				}
			);
		}

		public void LoadEntityInfo() {
			Load(
				delegate( XmlDocument doc ) {
					ConfigLoader.Instance.LoadEntities( this, doc );
				} 
			);
		}

		#endregion

    };

}

