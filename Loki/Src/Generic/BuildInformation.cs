using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Generic {

	public class BuildInformation {

		#region Private Fields

		private string name;
		private string guid;
		private List<string> assemblies = new List<string>();
		private List<BuildFile> files = new List<BuildFile>();
		private List<string> gacAssemblies = new List<string>();
		private List<string> resources = new List<string>();
		private List<ReferenceInformation> references = new List<ReferenceInformation>();
		private ProjectTypes projectType = ProjectTypes.Library;

		#endregion

		#region Properties

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string Guid {
			get { return guid; }
			set { guid = value; }
		}

		public List<string> Assemblies {
			get { return assemblies; }
			set { assemblies = value; }
		}

		public List<BuildFile> Files {
			get { return files; }
			set { files = value; }
		}

		public List<string> GacAssemblies {
			get { return gacAssemblies; }
			set { gacAssemblies = value; }
		}

		public List<ReferenceInformation> References {
			get { return references; }
			set { references = value; }
		}

		public ProjectTypes ProjectType {
			get { return projectType; }
			set { projectType = value; }
		}

		public List<string> Resources {
			get { return resources; }
			set { resources = value; }
		}

		#endregion

		#region Constructor

		public BuildInformation( string name ) {
			Name = name;
			guid = System.Guid.NewGuid().ToString().ToUpper();
		}

		#endregion
	}
}
