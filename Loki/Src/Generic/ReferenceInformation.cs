using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Generic {
	public class ReferenceInformation {

		private string name;
		private string guid;

		public string Name {
			get { return name; }
			set { name = value; }
		}
		
		public string Guid {
			get { return guid; }
			set { guid = value; }
		}

		public ReferenceInformation( string name, string guid ) {
			this.name = name;
			this.guid = guid;
		}

	}
}
