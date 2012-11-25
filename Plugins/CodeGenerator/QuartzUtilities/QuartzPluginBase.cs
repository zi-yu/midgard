using System;
using System.Collections.Generic;
using System.Text;
using Loki.Generic;
using Loki.Interfaces;
using System.IO;

namespace Quartz {
	public abstract class QuartzPluginBase : PluginBase, ICodeGenerator {

		#region Protected

		private string GetOutputDir() {
			string dir = ".";
			if (pluginParameters != null && pluginParameters.ContainsKey("OutputDir")) {
				dir = pluginParameters["OutputDir"];
			}

			string output = Project.OutputPath;
			if (dir != ".") {
				output = Path.Combine(output, dir);
			}

			return output;
		}
		
		protected string GetProcessDir( Process process ) {
			string dir = ".";
			if( pluginParameters != null && pluginParameters.ContainsKey( "OutputDir" ) ) {
				dir = pluginParameters["OutputDir"];
			}

			string output = Project.OutputPath;
			if( dir != "." ) {
				output = Path.Combine( output, dir );
			}

			return Path.Combine( output, process.Name  );
		}

		protected string GetRelativeOutputDir(string filename, string folder) {
			string output = Path.Combine(GetOutputDir(), folder);
			Directory.CreateDirectory(output);
			return new FileInfo(Path.Combine(output, filename)).FullName;
		}
		
		#endregion

		#region ICodeGenerator

		public virtual void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
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

		public string GenericFolder {
			get { return "Generic"; }
		}

        public List<Process> ProcessList
        {
            get
            {
                List<Process> process = (List<Process>)Project.Generic["Quartz"];
                if (process == null)
                {
                    throw new Exception("No Quartz information @ Project");
                }
                return process;
            }
        }

       	#endregion
	}
}
