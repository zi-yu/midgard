using System;
using System.Collections.Generic;
using System.Text;

namespace WebUtilities {

	internal class Dependencies {

		#region Private
		
		private static Dependencies instance = new Dependencies();
		private List<string> dependency = new List<string>();

		#endregion

		#region Properties

		public static Dependencies Instance {
			get { return instance; }
		}
	
		#endregion

		#region Public

		public void RegistDependency( string reference ) {
			dependency.Add( reference );
		}

		public IList<string> GetDependencies() {
			return dependency;
		}

		public bool HasDependency( string dep ) {
			return dependency.Contains( dep );
		}

		#endregion
	}
}
