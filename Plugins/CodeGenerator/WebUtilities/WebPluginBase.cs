using System;
using System.Collections.Generic;
using System.Text;
using Loki.Generic;
using Loki.Interfaces;
using System.IO;

namespace WebUtilities {
	public abstract class WebPluginBase : PluginBase, ICodeGenerator {

		#region Protected
		
		protected string GetOutputDir() {
			string dir = ".";
			if( pluginParameters != null && pluginParameters.ContainsKey( "OutputDir" ) ) {
				dir = pluginParameters["OutputDir"];
			}

			string output = Project.OutputPath;
			if( dir != "." ) {
				output = Path.Combine( output, dir );
			}

			return Path.Combine( output, ComponentType.WebUserInterface.ToString() );
		}

		protected string GetRelativeOutputDir( string filename, string folder ) {
			string output = Path.Combine( GetOutputDir(), folder );
			Directory.CreateDirectory( output );
			return new FileInfo( Path.Combine( output, filename ) ).FullName;
		}

        protected string GetControlsOutputDir(string filename) {
            string output = Path.Combine(GetOutputDir(), "Controls");
            Directory.CreateDirectory(output);
            return new FileInfo(Path.Combine(output, filename)).FullName;
        }

        protected string GetControlsOutputDir(string folder, string filename)
        {
            string output = Path.Combine(GetOutputDir(), "Controls");
            Directory.CreateDirectory(output);
            output = Path.Combine(output, folder);
            Directory.CreateDirectory(output);
            return new FileInfo(Path.Combine(output, filename)).FullName;
        }

		protected string GetRootOutputDir( string filename ) {
			string output = GetOutputDir();
			Directory.CreateDirectory( output );
			return new FileInfo( Path.Combine( output, filename ) ).FullName;
		}

		
		#endregion

		#region ICodeGenerator

		public virtual void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			Project = project;
			Aggregator = aggregator;
            this.Dependencies = dependencies;
            this.Dependencies.RegistDependency( Name );

			Aggregator.RegisterComponent( ComponentType.WebUserInterface.ToString() );
			Aggregator.RegisterProjectType( ComponentType.WebUserInterface.ToString(), ProjectTypes.Web );

            WebUtilities.Dependencies.Instance.RegistDependency(Name);
		}

		public virtual void BeforeGenerate() { }

		public virtual void Generate() { }

		public virtual void AfterGenerate() { }

		public override Dictionary<string, string> DefaultParameters {
			get {
				return new Dictionary<string, string>();
			}
		}
		
		#endregion

		#region Properties

		public string Namespace {
			get {
				return string.Format( "{0}.{1}", Project.Name, ComponentType.WebUserInterface.ToString() );
			}
		}

		public string ControlsNamespace {
			get {
				return string.Format( "{0}.{1}.Controls", Project.Name, ComponentType.WebUserInterface.ToString() );
			}
		}

		public string ModulesNamespace {
			get {
				return string.Format( "{0}.{1}.Modules", Project.Name, ComponentType.WebUserInterface.ToString() );
			}
		}

        public string Assembly
        {
            get
            {
                return string.Format("{0}.{1}", Project.Name, ComponentType.WebUserInterface.ToString());
            }
        }

        public string AdminNamespace
        {
            get
            {
                return string.Format("{0}.{1}.Admin", Project.Name, ComponentType.WebUserInterface.ToString());
            }
        }

        public string AdminControlsNamespace
        {
            get
            {
                return string.Format("{0}.{1}.Admin.Controls", Project.Name, ComponentType.WebUserInterface.ToString());
            }
        }


		#endregion
	}
}
