using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz {
    public class Node : INode {

        #region Fields

        protected string name = string.Empty;
		protected string label = string.Empty;
		protected List<Transiction> transictions = new List<Transiction>();
		protected string assembly = string.Empty;
		protected string nameSpace = string.Empty;
		protected Dictionary<string, string> extended = null;

        #endregion

		#region INode Members

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string Label {
			get { return label; }
			set { label = value; }
		}

        public string HtmlFormatedLabel {
            get { return Label.Replace("\n", "<br/>\n\t/// "); }
        }

		public string NameSpace {
			get { return nameSpace; }
		}

		public string Assembly {
			get { return assembly; }
		}

		public List<Transiction> Transictions {
			get { return transictions; }
		}

		public Dictionary<string, string> Extended {
			get { return extended; }
		}
	
		#endregion

		#region Private

		private bool HasTransiction( Transiction transiction) {
			foreach (Transiction t in Transictions) {
				if (t.EventName == transiction.EventName) {
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Public

		public void SetMoniker(string moniker) {
			string[] monikerArray = moniker.Split(',');
			
			string name = monikerArray[0];
			int end = name.LastIndexOf(".");

			nameSpace = name.Substring(0,name.Length-end-1);
			assembly = monikerArray[1];
		}

		public void AddExtended(string key, string value) {
			if (extended == null) {
				extended = new Dictionary<string, string>();
			}

			extended[key] = value;
		}

		public void AddTransiction(Transiction t) { 
			if( !HasTransiction(t) ) {
				Transictions.Add( t );
			}
		}
				 
		#endregion

    }
}
